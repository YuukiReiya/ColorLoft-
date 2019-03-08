#define KEYBOARD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// プレイヤークラス
/// </summary>
public class Player : MonoBehaviour {

    //  serialize param!
    [Header("Private Parameter"),Space(3)]
    [Header("Controller Parameter")]
    [SerializeField,Tooltip("使用するコントローラーのインデックス")]
    private GamePadInput.GamePad.Index useControllerIndex;
    [Header("Model Parameter")]
    [SerializeField, Tooltip("プレイヤーとして認識するモデル")] GameObject modelObject;
    [SerializeField, Tooltip("認識するモデルのアニメーター")] Animator animator;
    [Header("Control Parameter")]
    [SerializeField, Range(0.5f, 10.0f), Tooltip("移動速度")] float moveSpeed;
    [SerializeField, Range(1.0f, 10.0f), Tooltip("回転速度")] float rotSpeed;
    [SerializeField, Tooltip("ダッシュ時の移動速度の倍率")] float dashMag = 1;
    [SerializeField, Tooltip("攻撃のフレーム")] float attackFrame = 70;
    [SerializeField] LookAtTargetIK lookAtTargetIK;
    [Header("Attack Parameter")]
    [SerializeField] GameObject attackTrigger;
    [SerializeField, Tooltip("キックの威力(吹っ飛びの力)")] float attackPower = 10;
    //[SerializeField, Tooltip("アニメーション再生後、攻撃トリガーを出現させるまでのフレーム")] float attackTriggerStartFrame = 20;
    //[SerializeField, Tooltip("攻撃トリガー出現後、判定を無くすまでのフレーム")] float attackTriggerEndFrame = 30;
    //[SerializeField, Tooltip("トリガー消失後、アニメーションの終了まで待機するフレーム")] float waitAttackAnimationFrame = 0;
    [SerializeField] bool isAlwaysDrawGizmos = true;
    [Header("Damage Parameter")]
    [SerializeField, Tooltip("吹っ飛んでいるフレーム")] float blowOffFrame = 10;
    [SerializeField, Tooltip("混乱しているフレーム")] float confusionFrame = 240;
    [SerializeField] UnityChan.FaceUpdate faceUpdate;
    [Header("Invisible Parameter")]
    [SerializeField, Tooltip("無敵フレーム")] float invisibleFrame = 100;
    [SerializeField, Tooltip("無敵時のエフェクト")] ParticleSystem invisibleEffect;
    [Header("Outline Parameter")]
    [SerializeField, Tooltip("身体のレンダラー")] SkinnedMeshRenderer bodyMesh;

    //  private param!
    private GamePadController gamePad;
    private bool isAttack;
    private bool isInvisible;
    private IEnumerator damageCoroutine;
    private IEnumerator invisibleCoroutine;
    private int voiceID;                        //どのキャラのボイスなのか

#if UNITY_EDITOR
    private bool isDrawAttackTrigger;
#endif 
    private int point;

    //  public param!
    [Header("Public Parameter"),Space(3)]
    public DataBase.COLOR color = DataBase.COLOR.RED;

    //  const param!
    const int DAMAGE_PRECISION = 5;        //ダメージ処理の精度(このフレーム待機してからポイントを放出する)
    const float RELEASE_POINT_HEIGHT = 3;  //ポイントを放出する際の高さ
    const int BODY_MESH_CHILD_CODE = 0;    //頭体メッシュがモデルオブジェクトの何番目の子オブジェクトなのかを表す番号
    const int HEAD_MESH_CHILD_CODE = 3;    //頭メッシュがモデルオブジェクトの何番目の子オブジェクトなのかを表す番号
    const int BODY_OUTLINE_INDEX = 3;      //体メッシュにアタッチされているアウトライン用のマテリアルのインデックス
    const int HEAD_OUTLINE_INDEX = 1;      //頭メッシュにアタッチされているアウトライン用のマテリアルのインデックス
    const int HEAD_OUTLINE_INDEX_UTC = 3;  //UTCの頭メッシュにアタッチされているアウトライン用のマテリアルのインデックス
    const float VOICE_VOLUME = 1.0f;       //ボイスの音量

    //  property
    Vector3 resetPos { get; set; }
    public GamePadInput.GamePad.Index ContollerIndex { get { return useControllerIndex; } }
    public Vector3 Position { get { return modelObject.transform.position; } }
    public bool isMove { get; set; }
    public bool isRotation { get; set; }
    public int Point { get { return point; } }      //ポイント
    public int Stock { get; set; }                  //ストック数


    /// <summary>
    /// リセット関数
    /// </summary>
    private void Reset()
    {
        faceUpdate = transform.GetComponentInChildren<UnityChan.FaceUpdate>();
    }

#if UNITY_EDITOR
    /// <summary>
    /// ギズモ表示
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        if (isAlwaysDrawGizmos || isDrawAttackTrigger)
        {
            Vector3 pos = attackTrigger.transform.position;
            Gizmos.DrawWireCube(pos, attackTrigger.transform.localScale);
        }
    }
#endif

    public void DebugInit()
    {
        //  移動可能フラグ
        isMove = true;

        //  回転可能フラグ
        isRotation = true;

        //  攻撃可能フラグ
        isAttack = false;

        //  無敵フラグ
        isInvisible = false;

        //  初期位置の記憶
        resetPos = modelObject.transform.position;

        //  ポイント
        point = 0;

        //  コントローラーの設定
        gamePad = MyInputManager.GetController(useControllerIndex);

        //  蹴り
        KickEvent inst = modelObject.AddComponent<KickEvent>();
        inst.Init(animator);

        //  キック時の攻撃トリガーを入れる関数
        Action kickStartFunc = () =>
        {
            //  攻撃のトリガー
            isAttack = true;
            #region DEBUG
#if UNITY_EDITOR
            //  デバッグ用
            isDrawAttackTrigger = true;
#endif
            #endregion
        };
        inst.SetKickStartFunc(kickStartFunc);

        //  キック時の攻撃トリガーを切る関数
        Action kickEndFunc = () =>
        {
            AttackTrgOff();
        };
        inst.SetKickEndFunc(kickEndFunc);

        //  攻撃時に発生するボイス
        Action kickVoiceFunc = () =>
        {
            KickVoice();
        };
        inst.SetKickVoiceFunc(kickVoiceFunc);

        //  注視処理
        Vector3 targetPos = Camera.main.transform.position;
        lookAtTargetIK.SetTargetPosition(targetPos);
        lookAtTargetIK.SetLookAtWeight(1.0f, 0.0f, 0.45f, 0.0f, 0.5f);


    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        //  移動可能フラグ
        isMove = true;

        //  回転可能フラグ
        isRotation = true;

        //  攻撃可能フラグ
        isAttack = false;

        //  無敵フラグ
        isInvisible = false;

        //  初期位置の記憶
        resetPos = modelObject.transform.position;

        //  ポイント
        point = 0;

        //  コントローラーの設定
        gamePad = MyInputManager.GetController(useControllerIndex);

        //  アウトラインマテリアルの設定
        SetOutlineColor();

        //  蹴り
        KickEvent inst = modelObject.AddComponent<KickEvent>();
        inst.Init(animator);

        //  キック時の攻撃トリガーを入れる関数
        Action kickStartFunc = () =>
        {
            //  攻撃のトリガー
            isAttack = true;
            #region DEBUG
            #if UNITY_EDITOR
            //  デバッグ用
            isDrawAttackTrigger = true;
            #endif
            #endregion
        };
        inst.SetKickStartFunc(kickStartFunc);

        //  キック時の攻撃トリガーを切る関数
        Action kickEndFunc = () =>
        {
            AttackTrgOff();
        };
        inst.SetKickEndFunc(kickEndFunc);

        //  攻撃時に発生するボイス
        Action kickVoiceFunc = () =>
        {
            KickVoice();
        };
        inst.SetKickVoiceFunc(kickVoiceFunc);

        //  注視処理
        Vector3 targetPos = Camera.main.transform.position;
        lookAtTargetIK.SetTargetPosition(targetPos);
        lookAtTargetIK.SetLookAtWeight(1.0f, 0.0f, 0.45f, 0.0f, 0.5f);

    }

    public int GetContoroller()
    {
        return (int)useControllerIndex;
    }

    /// <summary>
    /// 使用するコントローラーの設定
    /// </summary>
    /// <param name="index"></param>
    public void SetControllerIndex(GamePadInput.GamePad.Index index)
    {
        useControllerIndex = index;
        gamePad = MyInputManager.GetController(useControllerIndex);
    }

    /// <summary>
    /// プレイヤーのモデルを設定
    /// </summary>
    /// <param name="model"></param>
    public void SetPlayerModel(GameObject model)
    {
        if (!model) { Debug.LogError("player model is null!"); }
        modelObject = model;
    }

    /// <summary>
    /// 色の設定
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(DataBase.COLOR color)
    {
        this.color = color;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void PlayerUpdate()
    {
        //コントローラー
        gamePad = MyInputManager.GetController(useControllerIndex);



        //攻撃
        //Attack();
        AttackTrgOn();
        AttackUpdate();

        //回転
        Turn();

        //移動
        Move();

    }

    /// <summary>
    /// 攻撃トリガーを入れる
    /// </summary>
    void AttackTrgOn()
    {
        //  B攻撃
        if (!gamePad.B) { return; }

        //  攻撃中には入らないようにする
        if (isAttack) { return; }

        //  ダメージを受けている最中も入らないようにする
        if (damageCoroutine != null) { return; }

        isMove = false;
        isRotation = false;

        //アニメーション遷移命令
        animator.SetBool("isAttack", true);
    }

    /// <summary>
    /// 攻撃処理の更新
    /// </summary>
    void AttackUpdate()
    {
        //  攻撃フラグが立っていなければ処理しない
        if (!isAttack) { return; }

        isRotation = false;
        isMove = false;

        //  複数回通る必要がない
        animator.SetBool("isAttack", false);

        if (damageCoroutine != null)
        {
            isAttack = false;
            isRotation = true;
            isMove = true;
            return;
        }

        Vector3 pos = attackTrigger.transform.position;             //OverlapBoxの中心
        Vector3 hScale = attackTrigger.transform.localScale / 2;    //OverlapBoxの大きさ(引数に合わせ半分)
        var playerColliders = Physics.OverlapBox(pos, hScale, modelObject.transform.rotation, (1 << 9), QueryTriggerInteraction.Ignore);

        //ダメージを受けたら当たり判定を途中で抜ける
        if (damageCoroutine != null) { return; }

        // オーバーラップボックスに含まれているプレイヤー分回す
        foreach (var it in playerColliders)
        {
            //対象
            Player target = it.transform.parent.GetComponent<Player>();

            //無敵状態なら吹っ飛ばさない
            if (target.isInvisible) { continue; }

            //既に吹っ飛んでいる相手に対しては吹っ飛ばさない
            if (target.damageCoroutine != null) { continue; }

            //吹き飛ばし処理の開始
            BlowOff(target);
        }

    }

    /// <summary>
    /// 攻撃トリガーを切る
    /// </summary>
    void AttackTrgOff()
    {
        isAttack = false;
        isMove = true;
        isRotation = true;

        #region DEBUG
#if UNITY_EDITOR
        isDrawAttackTrigger = false;
#endif
        #endregion

        //ダメージを受けていたらダメージ処理でフラグのリセットをする
        if (damageCoroutine == null)
        {
            isAttack = false;
            isMove = true;
            isRotation = true;
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    void Attack()
    {
        //  B攻撃
        if (!gamePad.B) { return; }

        //  攻撃中には入らないようにする
        if (isAttack) { return; }

        //初期化
        isAttack = true;
        animator.SetBool("isAttack", true);
        isMove = false;
        isRotation = false;
        StopMoveAnimation();
        KickVoice();



        StartCoroutine(AttackCoroutine());
    }

    /// <summary>
    /// 攻撃処理を行うコルーチン
    /// 攻撃中は硬直でキャンセルはダメージを受けたときのみ
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCoroutine()
    {
        yield return null;
        //
        isAttack = true;
        animator.SetBool("isAttack", true);
        isMove = false;
        isRotation = false;
        StopMoveAnimation();

        KickVoice();
        //アニメーションをオーバーラップをとり始めるフレームまで待機
        //for(int i = 0; i < attackTriggerStartFrame; ++i) {
        //    yield return null;
        //}

        ////  蹴りボイス

        #region DEBUG
#if UNITY_EDITOR
        isDrawAttackTrigger = true;
#endif
        #endregion

        //オーバーラップで攻撃が当たっているトリガーを判定
        //for (int i = 0; i < attackTriggerEndFrame; ++i)
        //{
        //    Vector3 pos = attackTrigger.transform.position;
        //    Vector3 hScale = attackTrigger.transform.localScale / 2;
        //    var playerColliders = Physics.OverlapBox(pos,hScale , modelObject.transform.rotation, (1 << 9), QueryTriggerInteraction.Ignore);

        //    //ダメージを受けたら当たり判定を途中で抜ける
        //    //if (damageCoroutine != null) { break; }

        //    foreach (var it in playerColliders)
        //    {
        //        //吹き飛ばし処理
        //        //Debug.Log("蹴ったったで!");
        //        //Debug.Log(it.gameObject.name+" を蹴った");

        //        Player target = it.transform.parent.GetComponent<Player>();

        //        //無敵状態なら吹っ飛ばさない
        //        if (target.isInvisible) { continue; }

        //        //既に吹っ飛んでいる相手に対しては吹っ飛ばさない
        //        if (target.damageCoroutine != null) { continue; }

        //        BlowOff(target);
        //    }
        //    yield return null;
        //}

        #region DEBUG
#if UNITY_EDITOR
        isDrawAttackTrigger = false;
#endif
        #endregion

        //アニメーションのフラグをリセット
        animator.SetBool("isAttack", false);

        //オーバーラップを無効にし、アニメーション終了まで待機するフレーム
        //for (int i = 0; i < waitAttackAnimationFrame; ++i) { yield return null; }

        //ダメージを受けていたらダメージ処理でフラグのリセットをする
        if (damageCoroutine == null)
        {
            isAttack = false;
            isMove = true;
            isRotation = true;
        }
    }

    /// <summary>
    /// 吹き飛ばし
    /// </summary>
    /// <param name="player"></param>
    void BlowOff(Player player)
    {
        //吹き飛ばすオブジェクトの座標
        Vector3 targetPos = player.transform.GetChild(0).position;

        //攻撃の中心
        Vector3 attackPos = modelObject.transform.position;

        //吹き飛ばす方向
        targetPos.y = attackPos.y = 0;
        Vector3 dir = (targetPos - attackPos).normalized;

        //エフェクト
        Vector3 fxPos = targetPos;
        fxPos.y += 1.5f;
        HitEffectsPool.Instance.Run(fxPos);

        //プレイヤーのオブジェクト
        GameObject obj = player.transform.GetChild(0).gameObject;

        //既にダメージを受けている相手に対しては再度ダメージ処理を受けなおしてもらう
        if (player.damageCoroutine != null)
        {
            //既に行われているダメージ処理の停止
            StopCoroutine(player.damageCoroutine);
        }

        //呼び出すダメージ処理を記憶
        player.damageCoroutine = player.DamageCoroutine();

        //相手プレイヤーに対してのダメージ処理の呼び出し
        StartCoroutine(player.damageCoroutine);

        //処理
        StartCoroutine(BlowOffCoroutine(obj, dir));
    }

    /// <summary>
    /// 吹っ飛びのコルーチン
    /// </summary>
    /// <param name="target"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    IEnumerator BlowOffCoroutine(GameObject target, Vector3 dir)
    {
        Vector3 pos = target.transform.position;

        Vector3 power= dir * Time.deltaTime * attackPower;

        for(int i = 0; i < blowOffFrame; ++i)
        {
            pos += power;
            target.transform.position = pos;
            yield return null;
        }
    }

    /// <summary>
    /// ダメージ処理のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageCoroutine()
    {
        //アニメーション
        //animator.SetTrigger("isDamage");
        //faceUpdate.OnCallChangeFace("damaged");

        //ボイス
        DamageVoice();

        //フラグを操作してダメージ中に動かないようにする
        isRotation = false;
        isMove = false;

        float confusionFrame = this.confusionFrame - DAMAGE_PRECISION;

        //ポイント放出まで待機
        for(int i = 0; i < DAMAGE_PRECISION; ++i)
        {
            yield return null;
        }

        //ポイント放出
        ReleasePoints();

        //ダメージ待機
        for (int i = 0; i <confusionFrame; ++i)
        {
            yield return null;
        }

        //フラグのリセット
        isRotation = true;
        isMove = true;

        //ダメージ処理の終了
        damageCoroutine = null;

        faceUpdate.OnCallChangeFace("default");
    }

    /// <summary>
    /// ポイントの放出
    /// </summary>
    void ReleasePoints()
    {
        //ポイントを持っていなかったら処理しない
        if (point <= 0) { return; }

        //出現座標
        Vector3 pos = modelObject.transform.position;
        pos.y = RELEASE_POINT_HEIGHT;

        //減らすポイント
        int hPoint = point / 2;

        //放出するポイント
        int rPoint = hPoint == 0 ? 1 : hPoint;

        //ポイント半分放出
        for (int i = 0; i < rPoint; ++i)
        {
            PointManager.Instance.CreateRandamPhysicsPoint(pos);
        }

        //UI表示
        PointUIPools.Instance.DrawNegativeScore(ContollerIndex, pos, rPoint);

        //ポイントを半分にする
        point -= rPoint;
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move() {

        //  動ける？
        if (!isMove) { return; }

        Vector2 norm = gamePad.LStick.normalized;       // 正規化した値  
        
        //  アニメーターの引数に変数をバインド
        float animSpeed = Mathf.Abs(norm.x) + Mathf.Abs(norm.y);
        animator.SetFloat("Speed", animSpeed);

        //  Aダッシュ
        float dash = gamePad.A_Hold ? dashMag : 1.0f;

        //  ジョイスティックの入力を正規化し移動させる
        Vector3 add = new Vector3(
            norm.x * moveSpeed * Time.deltaTime * dash,
            0,
            norm.y * moveSpeed * Time.deltaTime * dash
            );

        //  座標更新
        modelObject.transform.position += add;


#if KEYBOARD

        norm.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        norm.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        ;

        //  ジョイスティックの入力を正規化し移動させる
        add = new Vector3(
            norm.x * moveSpeed * Time.deltaTime,
            0,
            norm.y * moveSpeed * Time.deltaTime
            );

        //  座標更新
        modelObject.transform.position += add;

#endif
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    void Turn()
    {
        //回転できる？
        if (!isRotation) { return; }

        //  スティックの入力がない場合は回転値は更新しない
        if (gamePad.LStick == Vector2.zero) { return; }

        Vector2 norm = gamePad.LStick.normalized;       // 正規化した値  

        //  回転更新
        Vector3 eAngle = modelObject.transform.rotation.eulerAngles;

        float targetAngle = Mathf.Atan2(norm.y, -norm.x); //  スティックの倒した方向から向きを算出
        targetAngle *= Mathf.Rad2Deg;                     //  ディグリー角に変換
        targetAngle -= 90f;                               //  x軸からy軸(z軸)に値を変換

        //  回転を補完(線型補完)
        eAngle.y = Mathf.LerpAngle(eAngle.y, targetAngle,
            rotSpeed * Time.deltaTime);
        modelObject.transform.eulerAngles = eAngle;

    }

    /// <summary>
    /// 加点処理
    /// </summary>
    /// <param name="point"></param>
    public void AddPoint(int point = 1)
    {
        this.point += point;
    }

    /// <summary>
    /// 移動アニメーションの停止
    /// </summary>
    public void StopMoveAnimation()
    {
        animator.SetFloat("Speed", 0);
    }

    //ダメージ処理のキャンセル
    public void CancelDamage()
    {
        //アニメーションのキャンセル
        animator.SetTrigger("isCancelDamage");

        //コルーチンのキャンセル
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }

        //フラグのリセット
        isMove = true;
        isRotation = true;
        isAttack = false;
    }

    /// <summary>
    /// 無敵処理
    /// </summary>
    public void StartInvisible()
    {
        //既に無敵の状態だったら、無敵処理を止めてあげてから再度無敵にしてあげる
        if (invisibleCoroutine != null)
        {
            StopCoroutine(invisibleCoroutine);
        }

        //無敵処理の記憶
        invisibleCoroutine = InvisibleCoroutine();

        //無敵処理
        StartCoroutine(invisibleCoroutine);
    }

    /// <summary>
    /// エフェクト処理
    /// </summary>
    /// <returns></returns>
    IEnumerator InvisibleCoroutine()
    {
        //エフェクト処理
        invisibleEffect.Play();

        //無敵フラグを入れる
        isInvisible = true;

        //待機
        for(int i = 0; i < invisibleFrame; ++i) { yield return null; }

        //エフェクト停止
        invisibleEffect.Stop();

        //無敵フラグを切る
        isInvisible = false;

        //無敵状態終了
        invisibleCoroutine = null;
    }

    /// <summary>
    /// ポイントを半分にする
    /// </summary>
    public void PointHarf()
    {
        //ポイントを持っていなければ半分にしない
        if (point <= 0) { return; }

        //減らすポイント
        int hPoint = point / 2;

        //放出するポイント
        int rPoint = hPoint == 0 ? 1 : hPoint;

        //座標
        var pos = modelObject.transform.position;

        //UI表示
        PointUIPools.Instance.DrawNegativeScore(ContollerIndex, pos, rPoint);

        //ポイントを半分にする
        point -= rPoint;
    }

    /// <summary>
    /// 位置を初期位置にい戻す
    /// </summary>
    public void ResetPosition()
    {
        modelObject.transform.position = resetPos;
    }

    /// <summary>
    /// ボイスIDの登録
    /// </summary>
    /// <param name="id"></param>
    public void SetVoiceID(int id)
    {
        voiceID = id;
    }

    /// <summary>
    /// ダメージボイス
    /// </summary>
    void DamageVoice()
    {
        //
        string key = string.Empty;

        switch (voiceID)
        {
            //こはくちゃん
            case (int)DataBase.MODEL.UTC_S:
            case (int)DataBase.MODEL.UTC_W:
                key = "KohakuDamage";
                break;

            //みさきちゃん
            case (int)DataBase.MODEL.MISAKI_S:
            case (int)DataBase.MODEL.MISAKI_W:
                key = "MisakiDamage";
                break;

            //ゆうこちゃん
            case (int)DataBase.MODEL.YUKO_S:
            case (int)DataBase.MODEL.YUKO_W:
                key = "YukoDamage";
                break;
        }

        //
        SoundManager.Instance.PlayOnSE(key, VOICE_VOLUME);
    }

    void KickVoice()
    {
        string key = string.Empty;

        switch (voiceID)
        {
            //こはくちゃん
            case (int)DataBase.MODEL.UTC_S:
            case (int)DataBase.MODEL.UTC_W:
                key = "KohakuKick";
                break;

            //みさきちゃん
            case (int)DataBase.MODEL.MISAKI_S:
            case (int)DataBase.MODEL.MISAKI_W:
                key = "MisakiKick";
                break;

            //ゆうこちゃん
            case (int)DataBase.MODEL.YUKO_S:
            case (int)DataBase.MODEL.YUKO_W:
                key = "YukoKick";
                break;
        }

        //
        SoundManager.Instance.PlayOnSE(key, VOICE_VOLUME);
    }

    [ContextMenu("Search Invisible Effect")]
    void SearchEffect()
    {
        var meshes = transform.GetChild(0);
        var bodyMesh = meshes.GetChild(0);
        var fxTrans = bodyMesh.GetChild(0);

        invisibleEffect = fxTrans.GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// アウトラインの色を設定
    /// </summary>
    private void SetOutlineColor()
    {
        //体のメッシュ
        var bodyMesh = modelObject.transform.GetChild(BODY_MESH_CHILD_CODE);

        //体のアウトライン
        var bodyOutlineMat = bodyMesh.GetComponent<SkinnedMeshRenderer>().materials[BODY_OUTLINE_INDEX];

        //頭のメッシュ
        var headMesh = modelObject.transform.GetChild(HEAD_MESH_CHILD_CODE);

        //頭のアウトライン
        int headMatIndex = -1;

        //UTCだけ特殊なのでswitchで分ける
        switch (voiceID)
        {
            //こはくちゃん
            case (int)DataBase.MODEL.UTC_S:
            case (int)DataBase.MODEL.UTC_W:
                headMatIndex = HEAD_OUTLINE_INDEX_UTC;
                break;

            //みさきちゃん
            case (int)DataBase.MODEL.MISAKI_S:
            case (int)DataBase.MODEL.MISAKI_W:
            //ゆうこちゃん
            case (int)DataBase.MODEL.YUKO_S:
            case (int)DataBase.MODEL.YUKO_W:
                headMatIndex = HEAD_OUTLINE_INDEX;
                break;
        }
        var headOutlineMat = headMesh.GetComponent<SkinnedMeshRenderer>().materials[headMatIndex];

        //マテリアルの色を変更
        Color outlineColor = PlayerManager.Instance.ConvertOutlineColor(useControllerIndex);
        bodyOutlineMat.SetColor("_OutlineColor", outlineColor);
        headOutlineMat.SetColor("_OutlineColor", outlineColor);
    }
}

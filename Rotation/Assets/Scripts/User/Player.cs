#define KEYBOARD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤークラス
/// </summary>
public class Player : MonoBehaviour {


    //  private param!
    [SerializeField,Tooltip("使用するコントローラーのインデックス")]
    private GamePadInput.GamePad.Index useControllerIndex;

    [SerializeField, Tooltip("プレイヤーとして認識するモデル")] private GameObject modelObuject;
    [SerializeField, Tooltip("認識するモデルのアニメーター")] private Animator animator;

    [SerializeField, Range(0.5f, 10.0f), Tooltip("移動速度")] private float moveSpeed;
    [SerializeField, Range(1.0f, 10.0f), Tooltip("回転速度")] private float rotSpeed;
    [SerializeField, Tooltip("ダッシュ時の移動速度の倍率")] private float dashMag = 1;
     
    private GamePadController gamePad;
    private int point;

    //  public param!
    public DataBase.COLOR color = DataBase.COLOR.RED;


    //  property
    public Vector3 resetPos { get; private set; }
    public bool isMove { get; set; }

	// Use this for initialization
	void Start () {

        ////  コントローラー取得
        //gamePad = MyInputManager.GetController(useControllerIndex);

        ////  初期位置の記憶
        //resetPos = modelObuject.transform.position;

        ////移動可能フラグ
        //isMove = true;
	}
	
	// Update is called once per frame
	void Update () {


    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        //移動可能フラグ
        isMove = true;

        Debug.Log(MyInputManager.GetController(useControllerIndex).gameObject.name);
        //コントローラーの設定
        gamePad = MyInputManager.GetController(useControllerIndex);
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
        modelObuject = model;
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


        if (gamePad.BACK)
        {

            Debug.Log(modelObuject.name);
        }

        //回転
        Turn();
        //移動
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move() {

        if (!isMove) { return; }

        Vector3 pos = modelObuject.transform.position;  // モデルの座標
        Vector2 norm = gamePad.LStick.normalized;       // 正規化した値  
        
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
        modelObuject.transform.position += add;


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
        modelObuject.transform.position += add;

#endif
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    void Turn()
    {
        //  スティックの入力がない場合は回転値は更新しない
        if (gamePad.LStick == Vector2.zero) { return; }

        Vector2 norm = gamePad.LStick.normalized;       // 正規化した値  

        //  回転更新
        Vector3 eAngle = modelObuject.transform.rotation.eulerAngles;

        float targetAngle = Mathf.Atan2(norm.y, -norm.x); //  スティックの倒した方向から向きを算出
        targetAngle *= Mathf.Rad2Deg;                     //  ディグリー角に変換
        targetAngle -= 90f;                               //  x軸からy軸(z軸)に値を変換

        //  回転を補完(線型補完)
        eAngle.y = Mathf.LerpAngle(eAngle.y, targetAngle,
            rotSpeed * Time.deltaTime);
        modelObuject.transform.eulerAngles = eAngle;

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
    /// 所持ポイント取得
    /// </summary>
    /// <returns></returns>
    public int GetPoint()
    {
        return point;
    }

    /// <summary>
    /// 移動アニメーションの停止
    /// </summary>
    public void StopMoveAnimation()
    {
        animator.SetFloat("Speed", 0);
    }

    /// <summary>
    /// 勝利アニメーション
    /// </summary>
    public void StartWinAnimation()
    {
        animator.SetTrigger("isWin");
    }

    /// <summary>
    /// 敗北アニメーション
    /// </summary>
    public void StartLoseAnimation()
    {
        animator.SetTrigger("isLose");
    }

}

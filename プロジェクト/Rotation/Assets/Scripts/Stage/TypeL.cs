using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// L字型Gridの管理
/// </summary>
public class TypeL : MonoBehaviour
{

    //  private param!
    List<GameObject> grids;
    Rigidbody rb;
    Vector3 pos;
    Collider[] cols;                //子オブジェクトのコライダー情報
    Material mat;                   //マテリアルの複製
    IEnumerator reviveCoroutine;    //落下コルーチン
    Vector3[] nestingPos;           //めり込み判定用overrapBoxの中心座標(子オブジェクトの座標)
    DataBase.COLOR color;           //色

    //  call back!
    delegate void Func();

    //  const param!
    const float BLINKER_WAIT_FRAME = 300;   //点滅が始まるまで待機するフレーム
    const int BLINKER_COUNT = 5;            //点滅回数
    const float BLINKER_INIT_FRAME = 60;    //点滅にかけるフレームの初期値
    const float BLINKER_FRAME_MAG = 0.8f;   //点滅にかけるフレームの上昇倍率

    //read only!
    readonly Vector3 OVERLAPBOX_SCALE_OFFSET = new Vector3(-0.5f, -0.5f, -0.5f);//OverLapBox判定の際のスケールのオフセット

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        //  create collider instance!
        cols = GetComponentsInChildren<Collider>();


        foreach (var it in cols)
        {
            Vector3 scale = it.transform.lossyScale;

            //オフセットの加算
            scale += OVERLAPBOX_SCALE_OFFSET;

            Gizmos.DrawWireCube(it.transform.position, scale);
        }

        //
    }
#endif

    // Use this for initialization
    void Start()
    {

        //  create list instance!
        grids = new List<GameObject>();

        //  grids data!
        var gridChildren = GetComponentsInChildren<Grid>(true);

        //  add management list!
        foreach (var it in gridChildren)
        {
            grids.Add(it.gameObject);
        }

        //  get material!
        mat = GetComponent<MeshRenderer>().material;

        //  create collider instance!
        cols = new Collider[gridChildren.Length];

        //  loop index!
        int index = 0;

        //  get collider of children!
        foreach (var it in gridChildren)
        {
            cols[index++] = it.GetComponent<Collider>();
        }

        //  get Rigidbody!
        rb = GetComponent<Rigidbody>();

        //  set color!
        color = transform.GetChild(0).GetComponent<Grid>().color;

        //  reset pos!
        pos = this.transform.position;
    }

    /// <summary>
    /// Gridのコライダーのアクティブを変更
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveGridCol(bool active)
    {
        foreach (var it in cols)
        {
            it.enabled = active;
        }
    }

    /// <summary>
    /// RigidBodyのConstraintsを設定
    /// </summary>
    /// <param name="constraints"></param>
    public void SetConstraints(RigidbodyConstraints constraints)
    {
        rb.constraints = constraints;
    }

    /// <summary>
    /// 座標のリセット(初期位置に)
    /// </summary>
    public void ResetPosition()
    {
        //初期位置
        this.transform.position = pos;

    }

    /// <summary>
    /// 遅延復活処理
    /// </summary>
    /// <param name="time"></param>
    public void StartRevival()
    {
        //点滅色変更
        Color color = StageManager.Instance.GetBlockColor(this.color).Value.blinkerColor;
        color.a = 0;
        SetMaterialColor(color);

        //復活中なら落下させない
        if (reviveCoroutine != null) { return; }

        //コルーチンの記憶
        reviveCoroutine = Revival();

        //復活コルーチン開始
        StartCoroutine(reviveCoroutine);
    }

    private IEnumerator Revival()
    {
        //コライダーを切る
        SetActiveGridCol(false);

        //点滅開始まで待機
        for (int i = 0; i < BLINKER_WAIT_FRAME; ++i) { yield return null; }

        //マテリアルの色を取得(配列内の全てが同じ色)
        var cr = StageManager.Instance.GetBlockColor(this.color).Value.blinkerColor;

        //α値を上げてから下げるために、α値を初期化
        cr.a = StageManager.Instance.GetBlockColor(this.color).Value.blinkerColor.a;

        int loopNum = BLINKER_COUNT - 1;
        int count = 0;
        float initAlpha = StageManager.Instance.GetBlockColor(this.color).Value.blinkerColor.a;
        float initFrame = BLINKER_INIT_FRAME;
        float mag = BLINKER_FRAME_MAG;

        //点滅
        while (count < loopNum)
        {
            float value = initAlpha / initFrame;

            //α値を上げる
            for (int i = 0; i < initFrame; ++i)
            {
                cr.a += value;
                mat.color = cr;
                yield return null;
            }

            //補正
            cr.a = initAlpha;
            mat.color = cr;
            yield return null;

            //α値を下げる
            for (int i = 0; i < initFrame; ++i)
            {
                cr.a -= value;
                mat.color = cr;
                yield return null;
            }

            //補正
            cr.a = 0;
            mat.color = cr;
            yield return null;


            initFrame = initFrame * mag;
            count++;
        }

        //アルファ値をなくす
        cr.a = 0;

        //最後、徐々にアルファ値を戻す
        while (cr.a < 1.0f)//アルファ値を徐々に戻す
        {
            float value = initAlpha / initFrame;
            cr.a += value;
            mat.color = cr;
            yield return null;
        }

        //プレイヤーのめり込み判定
        var player = NestingPlayers();//めり込んだプレイヤーを取得

        foreach(var it in player)
        {
            it.ResetPosition(); //めり込んだ時は初期位置に戻してあげる
            it.PointHarf();     //死亡と同じ扱いとしてポイントを半分にする
        }

        //復活
        cr = StageManager.Instance.GetBlockColor(this.color).Value.normalColor;
        SetActiveGridCol(true); //コライダーを入れる
        SetMaterialColor(cr);   //色

        //復活処理の停止
        reviveCoroutine = null;

    }

    /// <summary>
    /// マテリアルの色を変える
    /// </summary>
    /// <param name="cr"></param>
    private void SetMaterialColor(Color color)
    {
        if (mat.HasProperty("_Color"))
        {
            mat.SetColor("_Color", color);
        }
    }

    /// <summary>
    /// 復活中かを返す
    /// </summary>
    /// <returns>true:復活中、false:復活していない</returns>
    public bool GetIsRevival()
    {
        return reviveCoroutine != null;
    }

    /// <summary>
    /// めり込み判定されたプレイヤーを返す
    /// </summary>
    /// <returns></returns>
    public Player[] NestingPlayers()
    {
        Queue<Player> playerQueue = new Queue<Player>();

        foreach (var col in cols)
        {
            //OverLapBoxのスケール
            Vector3 scale = col.transform.lossyScale / 2;
            scale += OVERLAPBOX_SCALE_OFFSET;

            //判定内のコライダーを取得
            var nestingColliders = Physics.OverlapBox(col.transform.position, scale);

            //取得したコライダー分回す
            foreach (var it in nestingColliders)
            {
                //取得したコライダーの中でも親にプレイヤーのスクリプトを持つものをローカルキューに入れる
                Player player = it.transform.parent.GetComponent<Player>();

                //nullチェック
                if (player)
                {
                    playerQueue.Enqueue(player);
                }

            }

        }

        //キューの中から重複を削除
        var ret= playerQueue.Distinct().ToArray();

        return ret;
    }
}
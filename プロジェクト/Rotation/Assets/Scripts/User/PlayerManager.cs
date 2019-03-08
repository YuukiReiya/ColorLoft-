using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :SingletonMonoBehaviour<PlayerManager> {

    //const param!
    //create position!
    private readonly Vector3[] CREATE_POSITION =
    {
        new Vector3(1.5f, 0.5f, 1),         //左下
        new Vector3(20.5f, 0.5f, 12.75f),   //右上
        new Vector3(1.5f, 0.5f, 12.75f),    //左上
        new Vector3(20.5f, 0.5f, 1),        //右下
    };
    //create rotatision!
    private readonly Vector3[] CREATE_ROTATION =
    {
        new Vector3(0,0,0),    //左下
        new Vector3(0,180,0),  //右上
        new Vector3(0,180,0),  //左上
        new Vector3(0,0,0),    //右下
    };
    //Not Entry value!
    public static readonly int NOT_ENTRY = 0;

    //private param!
    [SerializeField,] Player[] playersPrefab;
    Player[] players;
    int[] id;

    //Debug
    [SerializeField] bool debug = false;


    /// <summary>
    /// 更新処理
    /// </summary>
    public void PlayersUpdate()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (id[i] == 0) { continue; }
            players[i].PlayerUpdate();
        }
    }

    /// <summary>
    /// プレイヤーの取得
    /// </summary>
    /// <param name="index">番号</param>
    /// <returns>取得したプレイヤー</returns>
    public Player GetPlayer(int index)
    {
        Player p = null;

        //下限をオーバー
        if (index < 0) { p = players[0]; }
        //上限をオーバー
        else if (players.Length <= index) { p = players[(players.Length - 1)]; }
        //エラー無し
        else  { p = players[index]; }

        return p;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        //インスタンス作成
        players = new Player[DataBase.PLAYER_NUM];
        id = new int[DataBase.PLAYER_NUM];

        for(int i = 0; i < players.Length; ++i)
        {
            players[i] = new Player();
        }
    }

    /// <summary>
    /// IDの設定とID関連の初期化
    /// </summary>
    /// <param name="id"></param>
    public void SetID(int id)
    {
        //登録されていないユーザーのIDの初期化は弾く！
        if (id == NOT_ENTRY) { return; }

        int index;                              //配列の添え字
        //添え字
        index = DataBase.GetControllerID(id) - 1;
        this.id[index] = id;
    }

    /// <summary>
    /// 登録情報からプレイヤーを生成
    /// </summary>
    public void CreatePlayer()
    {

#if UNITY_EDITOR
        if (debug)
        {
            DebugMode();
        }
#endif

        for (int i = 0; i < id.Length; ++i)
        {
            //IDが登録されて無ければ飛ばす
            if (id[i] == NOT_ENTRY) { continue; }

            int index;                              //配列の添え字
            int prefabNo;                           //プレハブ番号
            GamePadInput.GamePad.Index controller;  //コントローラーの番号
            DataBase.COLOR color;                   //色の設定

            //添え字
            index = DataBase.GetControllerID(id[i]) - 1;

            //プレイヤーのプレハブ
            prefabNo = DataBase.GetPrefabID(id[i]) - 1;
            var inst = Instantiate(playersPrefab[prefabNo]);
            inst.name = "Player" + index;
            inst.gameObject.transform.position = CREATE_POSITION[index];
            inst.gameObject.transform.rotation = Quaternion.Euler(CREATE_ROTATION[index]);
            players[index] = inst;

            //コントローラーの設定
            controller = (GamePadInput.GamePad.Index)DataBase.GetControllerID(id[i]);
            players[index].SetControllerIndex(controller);

            //色の設定
            color = (DataBase.COLOR)DataBase.GetColorID(id[i]);
            players[index].SetColor(color);

            //初期化
            players[i].SetVoiceID(DataBase.GetPrefabID(id[i]));
            players[index].Initialize();

#if UNITY_EDITOR
            if (debug)
            {
                players[index].Stock = 9999;
            }
#endif
        }

    }

    /// <summary>
    /// インスタンスを生成したプレイヤーを取得
    /// </summary>
    /// <returns></returns>
    public Player[] GetPlayers()
    {
        //Player[] ret;
        //int num = 0;

        ////エラーチェック
        //if (players == null) { return null; }

        ////生成するインスタンス数をカウント
        //for(int i = 0; i < players.Length; ++i)
        //{
        //    //未登録なら処理しない
        //    if (id[i] == NOT_ENTRY) { continue; }

        //    num++;
        //}

        ////インスタンス生成
        //ret = new Player[num];

        ////インデックス
        //int index = 0;


        //for(int i = 0; i < players.Length; ++i)
        //{
        //    //未登録なら処理しない
        //    if (id[i] == 0) { continue; }

        //    //戻り値型に登録されているもののみ入れる
        //    ret[index++] = players[i];
        //}

        ////IDが登録済みのもののみ返す
        //return ret;

        //エラーチェック
        if (players == null) return null;

        return players;

    }

    /// <summary>
    /// IDの取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetID(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return NOT_ENTRY;
        }
        //上限
        if (index > id.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top liit over" + "</color>");
            return NOT_ENTRY;
        }

        return id[index];
    }

    /// <summary>
    /// デバッグモード
    /// </summary>
    private void DebugMode()
    {
        Initialize();

        //ID
        for(int i = 0; i < id.Length; ++i)
        {
            int id_i, id_m, id_c;

            id_i = (i + 1) * DataBase.ID_INDEX;
            id_m = (i + 1) * DataBase.ID_PREFAB;
            id_c = i * DataBase.ID_COLOR;

            id[i] = id_i + id_m + id_c;
        }
    }

    /// <summary>
    /// プレイヤーのインデックスに対してアウトラインの色を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Color ConvertOutlineColor(GamePadInput.GamePad.Index index)
    {
        Color ret = new Color(0, 0, 0, 0);

        switch (index)
        {
            case GamePadInput.GamePad.Index.One:
                ret = Color.red;
                break;
            case GamePadInput.GamePad.Index.Two:
                ret = Color.blue;
                break;
            case GamePadInput.GamePad.Index.Three:
                ret = Color.green;
                break;
            case GamePadInput.GamePad.Index.Four:
                ret = Color.yellow;
                break;
            default:
                Debug.LogError("Invalid value");
                break;
        }

        return ret;
    }
}

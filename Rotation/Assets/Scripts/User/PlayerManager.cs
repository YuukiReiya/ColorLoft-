using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :SingletonMonoBehaviour<PlayerManager> {

    //const param!
    //create position!
    private readonly Vector3[] CREATE_POSITION =
    {
        new Vector3(1, 0.5f, 1),        //左下
        new Vector3(13, 0.5f, 12.75f),  //右上
        new Vector3(1, 0.5f, 12.75f),   //左上
        new Vector3(13, 0.5f, 1),       //右下
    };
    //create rotatision!
    private readonly Vector3[] CREATE_ROTATION =
    {
        new Vector3(0,0,0),    //左下
        new Vector3(0,180,0),  //右上
        new Vector3(0,0,0),    //左上
        new Vector3(0,180,0),  //右下
    };

    [SerializeField,] Player[] playersPrefab;

    Player[] players;
    int[] id;


    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < players.Length; ++i)
        {
            if (id[i] == 0) { continue; }
            Debug.Log(players[i].name);
            players[i].PlayerUpdate();
        }

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public void PlayersUpdate()
    {
        //foreach(var it in players)
        //{
        //    if (!it.gameObject) { continue; }
        //    it.PlayerUpdate();
        //}
        //for(int i = 0; i < players.Length; ++i)
        //{
        //    if (id[i] == 0) { continue; }
        //    Debug.Log(players[i].name);
        //    players[i].PlayerUpdate();
        //}
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

    private void Awake()
    {
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
        if (id == 0) { return; }

        int index;                              //配列の添え字
        //添え字
        index = DataBase.GetControllerID(id) - 1;
        this.id[index] = id;

        //int index;                              //配列の添え字
        //int prefabNo;                           //プレハブ番号
        //GamePadInput.GamePad.Index controller;  //コントローラーの番号
        //DataBase.COLOR color;                   //色の設定

        ////添え字
        //index = DataBase.GetControllerID(id) - 1;

        ////プレイヤーのプレハブ
        //prefabNo = DataBase.GetPrefabID(id);
        //players[index] = playersPrefab[prefabNo];

        ////コントローラーの設定
        //controller = (GamePadInput.GamePad.Index)DataBase.GetControllerID(id);
        //players[index].SetControllerIndex(controller);

        ////色の設定
        //color = (DataBase.COLOR)DataBase.GetColorID(id);
        //players[index].SetColor(color);
    }

    /// <summary>
    /// 登録情報からプレイヤーを生成
    /// </summary>
    public void CreatePlayer()
    {

        //Initialize();

        for(int i = 0; i < id.Length; ++i)
        {
            //IDが登録されて無ければ飛ばす
            if (id[i] == 0) { continue; }

            int index;                              //配列の添え字
            int prefabNo;                           //プレハブ番号
            GamePadInput.GamePad.Index controller;  //コントローラーの番号
            DataBase.COLOR color;                   //色の設定

            //添え字
            index = DataBase.GetControllerID(id[i]) - 1;

            //プレイヤーのプレハブ
            prefabNo = DataBase.GetPrefabID(id[i]);
            var inst = Instantiate(playersPrefab[prefabNo]);
            inst.gameObject.transform.position = CREATE_POSITION[index];
            inst.gameObject.transform.rotation = Quaternion.Euler(CREATE_ROTATION[index]);
            players[index] = playersPrefab[prefabNo];

            //コントローラーの設定
            controller = (GamePadInput.GamePad.Index)DataBase.GetControllerID(id[i]);
            players[index].SetControllerIndex(controller);

            //色の設定
            color = (DataBase.COLOR)DataBase.GetColorID(id[i]);

            //初期化
            players[index].Initialize();
        }

    }
}

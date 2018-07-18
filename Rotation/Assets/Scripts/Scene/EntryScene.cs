using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePadInput;

/// <summary>
/// プレイヤーのエントリーシーン
/// </summary>
public class EntryScene : IScene {

    /// <summary>
    /// 参加データが入った構造体
    /// </summary>
    public class EntryData
    {
        public GamePad.Index index;//コントローラー番号
        public bool registered;    //登録済みフラグ
        public int id;             //ID
        public bool isInput;       //スティックの連続入力防止フラグ
    }

    /// <summary>
    /// 定数宣言
    /// </summary>
    private const int RESET_FRAME = 15;//キー入力をリセットするフレーム

    //private param!
    EntryData[] data;   //参加者用データ
    bool isTransition;  //シーン遷移フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //遷移フラグ
        isTransition = false;

        //プレイヤーの準備
        PlayerManager.Instance.Initialize();

        //最大4人までなので4つ用意しておく！
        data = new EntryData[DataBase.PLAYER_NUM];
       
        //データを初期化
        for(int i = 0; i < data.Length; ++i)
        {
            data[i]             = new EntryData();
            data[i].index       = (GamePad.Index)(i + 1);
            data[i].registered  = false;
            data[i].id          = 0;
            data[i].isInput     = false;
        }

        //ビューワーの初期化
        ViewerManager.Instance.Initialize();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        EFinishInput(); //参加締め切り処理

        //シーン遷移のフラグを判定
        if (isTransition)
        {
            StartGame();
        }

        Entry();        //参加受付
        EAfterInput();  //参加者更新処理

        //モデルのビュー
        ViewerManager.Instance.View();
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

    /// <summary>
    /// 参加プレイヤーのエントリー
    /// </summary>
    public void Entry()
    {
        //foreach文だと書き換えできないためfor文
        for (int i = 0; i < data.Length; ++i)
        {
            //登録済みなら処理しない
            if (data[i].registered) { continue; }

            //登録
            if (MyInputManager.GetController(data[i].index).START)
            {
                data[i].registered = true;

                int id_i = (int)data[i].index * DataBase.ID_INDEX;          //コントローラー番号
                int id_m = (int)DataBase.MODEL.UTC_S * DataBase.ID_PREFAB;  //モデル番号(初期はUTC_S)
                int id_c = (int)data[i].index * DataBase.ID_COLOR;          //色番号(初期はコントローラー番号に応じた色が設定)

                //ID決定
                data[i].id = id_i + id_m + id_c;

                //一番最初のモデルビュー(UTC_S)
                ViewerManager.Instance.Create(data[i].index, 0);

            }
        }
    }

    /// <summary>
    /// エントリー後の操作
    /// </summary>
    public void EAfterInput()
    {
        //foreach文だと書き換えできないためfor文
        for (int i = 0; i < data.Length; ++i)
        {
            //登録してなければ処理しない
            if (!data[i].registered) { continue; }

            //キャラ選択
            CharacterChoice(data[i].index, data[i]);
        }
    }

    /// <summary>
    /// キャラクター選択
    /// </summary>
    /// <param name="index">コントローラー番号</param>
    /// <param name="data">エントリーデータ</param>
    void CharacterChoice(GamePad.Index index, EntryData data)
    {
        var input = MyInputManager.GetController(index);

        //IDからコントローラーインデックス取得
        int id_i = DataBase.GetControllerID(data.id);

        //IDから選択モデル取得
        int id_m = DataBase.GetPrefabID(data.id);

        int before = DataBase.GetPrefabID(data.id);//比較用変更前のモデル番号

        //左
        if (!data.isInput && input.LStick.x < 0)
        {
            id_m--;
            if ((DataBase.MODEL)id_m < DataBase.MODEL.UTC_S)
            {
                id_m = (int)DataBase.MODEL.YUKO_W;
            }

            //連続入力防止用フラグを立てる
            data.isInput = true;

            //キーフラグリセット用コルーチンの開始
            IEnumerator coroutine = ResetKeyFlags(data);
            CoroutineManager.Instance.StartCoroutineMethod(coroutine);

        }
        //右
        else if (!data.isInput && input.LStick.x > 0)
        {
            id_m++;
            if ((DataBase.MODEL)id_m > DataBase.MODEL.YUKO_W)
            {
                id_m = (int)DataBase.MODEL.UTC_S;
            }

            //連続入力防止用フラグを立てる
            data.isInput = true;

            //キーフラグリセット用コルーチンの開始
            IEnumerator coroutine = ResetKeyFlags(data);
            CoroutineManager.Instance.StartCoroutineMethod(coroutine);
        }

        id_i *= DataBase.ID_INDEX;
        id_m *= DataBase.ID_PREFAB;
        int id_c = DataBase.GetColorID(data.id);
        //IDの再設定
        data.id = id_i + id_m + id_c;

        //IDの変更があった場合モデルビューに表示するモデルを切り替える
        if (DataBase.GetPrefabID(data.id) != before)
        {
            ViewerManager.Instance.Create(index, (id_m / DataBase.ID_PREFAB - 1));
        }

    }

    /// <summary>
    /// スティックの連続入力防止用コルーチン
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator ResetKeyFlags(EntryData data)
    {
        for (int i = 0; i < RESET_FRAME; ++i) yield return null;
        data.isInput = false;
    }

    /// <summary>
    /// エントリー終了の操作
    /// </summary>
    private void EFinishInput()
    {
        //foreach文だと書き換えできないためfor文
        for (int i = 0; i < data.Length; ++i)
        {
            //登録してなければ処理しない
            if (!data[i].registered) { continue; }

            var input = MyInputManager.GetController(data[i].index);

            //ゲーム開始
            if (input.START)
            {
                isTransition = true;
                Debug.Log(data[i].id);
                break;
            }
        }

    }

    /// <summary>
    /// ゲームの開始
    /// </summary>
    private void StartGame()
    {
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.GAME);

        //IDの設定
        //foreach文だと書き換えできないためfor文
        for (int i = 0; i < data.Length; ++i)
        {
            PlayerManager.Instance.SetID(data[i].id);
        }
    }
}

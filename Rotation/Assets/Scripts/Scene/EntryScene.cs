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

    EntryData[] data;

    /// <summary>
    /// 定数宣言
    /// </summary>
    private const int RESET_FRAME = 15;//キー入力をリセットするフレーム

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //最大4人までなので4つ用意しておく！
        data = new EntryData[4];
       
        //
        for(int i = 0; i < data.Length; ++i)
        {
            data[i]             = new EntryData();
            data[i].index       = (GamePad.Index)(i + 1);
            data[i].registered  = false;
            data[i].id          = 0;
            data[i].isInput     = false;
        }
        ViewerManager.Instance.Initialize();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        Entry();
        EAfterInput();
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

                int id_m = (int)DataBase.MODEL.UTC_S * DataBase.ID_MODEL;   //モデル番号(初期はUTC_S)
                int id_c = (int)data[i].index * DataBase.ID_COLOR;          //色番号(初期はコントローラー番号に応じた色が設定)

                //ID決定
                data[i].id = id_m + id_c;

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

        //IDから選択モデル取得
        int id_m = data.id / DataBase.ID_MODEL;

        int before = DataBase.GetModelID(data.id);//比較用変更前のモデル番号

        //左
        if (!data.isInput && input.LStick.x < 0)
        // if (input.X)
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
        //else if (input.B)
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

        id_m *= DataBase.ID_MODEL;
        int id_c = DataBase.GetColorID(data.id);
        //IDの再設定
        data.id = id_m + id_c;


        //IDの変更があった場合モデルビューに表示するモデルを切り替える
        if (DataBase.GetModelID(data.id) != before)
        {
            ViewerManager.Instance.Create(index, (id_m / DataBase.ID_MODEL - 1));
        }

    }

    private IEnumerator ResetKeyFlags(EntryData data)
    {
        for (int i = 0; i < RESET_FRAME; ++i) yield return null;
        data.isInput = false;
    }
}

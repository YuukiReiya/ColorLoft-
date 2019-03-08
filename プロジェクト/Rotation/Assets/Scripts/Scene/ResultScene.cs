using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GamePadInput;

/// <summary>
/// リザルトシーン
/// </summary>
public class ResultScene : IScene {

    /// <summary>
    /// リザルト時のプレイヤーのデータが入った
    /// </summary>
    struct ResultData
    {
        public int id;          //ID
        public State state;     //入力状況
    }

    /// <summary>
    /// リザルト時のメニュー
    /// </summary>
    public enum Menu
    {
        Retry,
        Entry,
        Title,
    }

    /// <summary>
    /// リザルトシーン上での入力状況
    /// </summary>
    enum State
    {
        Wait,   //待機
        End,    //入力終了
    }

    /// <summary>
    /// 定数宣言
    /// </summary>
    private const int RESET_FRAME = 30;     //キー入力をリセットするフレーム
    private const float WAIT_FRAME = 60;    //参加者全員がSTARTボタンを押してシーン遷移が始まるまでの待機フレーム
    private const int iMENU_HEAD = (int)Menu.Retry;//メニュー列挙体先頭
    private const int iMENU_TAIL = (int)Menu.Title;//メニュー列挙体末尾

    //  private param!
    ResultData[] data;  //プレイヤーの入力状況を取得する構造体
    bool isTransition;  //シーン遷移フラグ
    Menu menu;          //メニュー
    bool isInput;       //連続入力防止
    IEnumerator transCoroutine;//シーン遷移のための変数

    //  property!
    bool isAllPlayerFinishedInput
    {
        get {

            //参加したプレイヤーの取得
            var entryPlayers = data.Where(it => it.id != PlayerManager.NOT_ENTRY).ToArray();

            //入力終了済みのプレイヤーを取得
            var finishPlayers = data.Where(it => it.state == State.End).ToArray();

            //誰一人入力し終わってない
            if (finishPlayers.Length == 0) { return false; }

            //全員が入力し終わっていたら
            return entryPlayers.Length == finishPlayers.Length;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //デバッグ
        if (PlayerManager.Instance.GetPlayers() == null)
        {
            PlayerManager.Instance.Initialize();
            PlayerManager.Instance.CreatePlayer();
        }
        RankManager.Instance.Initialize();


        var view = ViewerManager.Instance;
        view.Initialize();
        var pm = PlayerManager.Instance;

        //インスタンス生成
        data = new ResultData[DataBase.PLAYER_NUM];

        //プレイヤーのIDを受け取る
        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            data[i].id = pm.GetID(i);
        }

        //ビューモデルの初期化
        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            int id = data[i].id;
            GamePad.Index index = (GamePad.Index)DataBase.GetControllerID(id);
            int modelID = DataBase.GetPrefabID(id);

            //参加してないプレイヤーは飛ばす
            if (id == PlayerManager.NOT_ENTRY) continue;

            view.Create(index, modelID);
            view.SetRotationFlags(i, false);
        }

        //テキストの初期化
        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            int id = data[i].id;

            //参加してないプレイヤーは飛ばす
            if (id == PlayerManager.NOT_ENTRY)
            {
                ResultTextManager.Instance.SetNotEntryText(i);
            }
            //参加プレイヤー用テキスト
            else
            {
                ResultTextManager.Instance.SetWaitText(i);
            }
        }

        //連続入力防止
        isInput = false;

        //コルーチンの初期化
        transCoroutine = null;

        //メニュー
        menu = Menu.Entry;//キャラ選択

        //モデルの向きを調整
        view.LookAtFront();

        //獲得ポイントの表示
        RankManager.Instance.SetPoint();

        //アニメーション
        RankManager.Instance.StartResultAnimation();

        //BGM
        SoundManager.Instance.PlayOnBGM("BGM");

    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
       // bool before = isAllPlayerFinishedInput;

        //入力
        Input();

       // bool after = isAllPlayerFinishedInput;

        //フラグの更新された瞬間
        //if (before != after)
        //{
        //    //全員が入力し終わりメニュー選択に移った
        //    ResultMenuManager.Instance.Initialize();
        //}


        //ビューモデルを正面に向かせる
        ViewerManager.Instance.LookAtFront();

#if UNITY_EDITOR
        RankManager.Instance.DrawDebug();
#endif
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

    /// <summary>
    /// 入力処理
    /// </summary>
    void Input()
    {
        //全員が入力し終わっていたらメニュー処理
        if (isAllPlayerFinishedInput)
        {
            if (transCoroutine == null)
            {
                transCoroutine = TransCoroutine();
                CoroutineManager.Instance.StartCoroutineMethod(transCoroutine);
            }
        }
        //入力待ち処理
        else
        {
            WaitForInput();
        }

    }

    /// <summary>
    /// メニューの更新
    /// </summary>
    void MenuUpdate()
    {
        var input = MyInputManager.AllController;

        //メニューのの変更
        ChangeMenu();

        //メニュー決定処理
        if (input.A)
        {
            Transition();
        }
    }

    /// <summary>
    /// シーン遷移コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator TransCoroutine()
    {
        //待機
        for(int i = 0; i < WAIT_FRAME; ++i) { yield return null; }

        //シーン遷移
        Transition();
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    void Transition()
    {
        switch (menu)
        {
            //もういちど
            case Menu.Retry:
                SceneController.Instance.LoadFadeScene(SceneController.SCENE.GAME);
                break;

            //キャラクター選択
            case Menu.Entry:
                SceneController.Instance.LoadFadeScene(SceneController.SCENE.ENTRY);
                break;
            
            //タイトル
            case Menu.Title:
                SceneController.Instance.LoadFadeScene(SceneController.SCENE.TITLE);
                break;

        }
    }

    /// <summary>
    /// 選択メニュー項目の変更
    /// </summary>
    void ChangeMenu()
    {
        var input = MyInputManager.AllController;

        //スティック入力が立っていれば処理しない
        if (isInput) { return; }

        //現在メニューの取得
        int iMenu = (int)menu;

        //上
        if (input.LStick.y > 0)
        {
            //スティック入力のフラグを更新
            CoroutineManager.Instance.StartCoroutineMethod(ResetKeyFlags());

            --iMenu;
            if (iMenu < iMENU_HEAD)
            {
                iMenu = iMENU_TAIL;
            }
        }
        //下
        if (input.LStick.y < 0)
        {
            //スティック入力のフラグを更新
            CoroutineManager.Instance.StartCoroutineMethod(ResetKeyFlags());

            ++iMenu;
            if (iMenu > iMENU_TAIL)
            {
                iMenu = iMENU_HEAD;
            }
        }

        //設定項目であるメニューの更新
        menu = (Menu)iMenu;

        //選択メニューの更新
        ResultMenuManager.Instance.SetMenu(menu);
    }

    /// <summary>
    /// 入力待ち処理
    /// </summary>
    void WaitForInput()
    {
        //入力待ちプレイヤーを取得
        var players = data.Where(it => it.state == State.Wait).ToArray();

        foreach(var it in players)
        {
            if (it.id == PlayerManager.NOT_ENTRY) continue;

            int index = DataBase.GetControllerID(it.id) - 1;
            GamePad.Index gamePadIndex = (GamePad.Index)(index + 1);
            var input = MyInputManager.GetController(gamePadIndex);

            //入力終了の瞬間
            if (input.START)
            {
                data[index].state = State.End;
                ResultTextManager.Instance.StartCloseAndOpenVertical(index);
                ResultTextManager.Instance.SetFinishedText(index);

                //効果音
                SoundManager.Instance.PlayOnSE("Inputed");
            }
        }
    }

    /// <summary>
    /// スティックの連続入力防止用コルーチン
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator ResetKeyFlags()
    {
        isInput = true;
        for (int i = 0; i < RESET_FRAME; ++i) yield return null;
        isInput = false;
    }

}

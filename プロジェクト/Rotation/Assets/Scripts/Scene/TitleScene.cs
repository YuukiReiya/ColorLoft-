using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルシーン
/// </summary>
public class TitleScene : IScene {

    /// <summary>
    /// メニューの列挙体
    /// </summary>
    public enum Menu
    {
        Play,
        Credit,
        End
    }

    //  private param!
    Menu menu;
    bool isInput;
    bool isPushButton;

    //  const param!
    private const int SE_CHANNEL = 1;               //SE用のチャネル
    private const int RESET_FRAME = 30;             //キー入力をリセットするフレーム
    private const int iMENU_HEAD = (int)Menu.Play;  //メニュー列挙体先頭
    private const int iMENU_TAIL = (int)Menu.End;   //メニュー列挙体末尾

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        menu = Menu.Play;
        isInput = false;
        isPushButton = false;
        TitleUIManager.Instance.StartFloatText();


        //BGMチャネルの音量をゼロにしてあげる
        SoundManager.Instance.GetChannel(SoundManager.DEFAULT_CHANNEL_INDEX).volume = 0;

        //BGM
        float fadeFrame = SceneController.Instance.FadeFrame;   //フェードさせるフレーム
        SoundManager.Instance.StartSoundFadeOut(fadeFrame);     //フェードアウト命令
        SoundManager.Instance.PlayOnBGM("BGM");                 //BGM流す

    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        //入力
        Input();
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

    /// <summary>
    /// 入力
    /// </summary>
    private void Input()
    {
        //メニューが出ていない時
        if (!isPushButton)
        {
            PopUpMenu();
        }
        //メニューが出ている
        else
        {
            MenuUpdate();
        }
    }

    /// <summary>
    /// メニューの更新
    /// </summary>
    private void MenuUpdate()
    {
        var input = MyInputManager.AllController;

        //メニュー変更
        ChangeMenu();

        //メニュー決定処理
        if (input.A)
        {
            Transition();
        }
    }

    /// <summary>
    /// 選択メニュー項目の変更
    /// </summary>
    private void ChangeMenu()
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

            //効果音
            SoundManager.Instance.PlayOnSE("cursor");
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

            //効果音
            SoundManager.Instance.PlayOnSE("cursor");
        }

        //設定項目であるメニューの更新
        menu = (Menu)iMenu;

        //メニューUIの更新
        TitleUIManager.Instance.SetMenu(menu);

    }

    /// <summary>
    /// メニューをポップアップさせる
    /// </summary>
    private void PopUpMenu()
    {
        var input = MyInputManager.AllController;

        //何かボタンが押されたら
        if(input.A||input.B||input.X||input.Y||
            input.START || input.BACK)
        {
            //メニューを出現させる
            TitleUIManager.Instance.PopUpMenu();

            //効果音
            SoundManager.Instance.PlayOnSE("start");

            //フラグを切り替える
            isPushButton = true;
        }
    }

    /// <summary>
    /// シーン遷移
    /// </summary>
    private void Transition()
    {
        switch (menu)
        {
            //ゲーム開始
            case Menu.Play:
                GameStart();
                break;

            //クレジット
            case Menu.Credit:
                Credit();
                break;

            //ゲーム終了
            case Menu.End:
                GameEnd();
                break;
        }

        //音量のフェード
        float fadeFrame = SceneController.Instance.FadeFrame;
        SoundManager.Instance.StartSoundFadeIn(fadeFrame);
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    private void GameStart()
    {
        //効果音
        SoundManager.Instance.Play("decision", SE_CHANNEL);

        //シーン遷移
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.ENTRY);
    }

    /// <summary>
    /// クレジット
    /// </summary>
    private void Credit()
    {
        //効果音
        SoundManager.Instance.Play("decision", SE_CHANNEL);

        //シーン遷移
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.CREDIT);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    private void GameEnd()
    {
        //効果音
        SoundManager.Instance.Play("decision");

        //フェードインするフレーム
        int fadeFrame = SceneController.Instance.FadeFrame;

        //フェードアウト終了時に行う処理
        FadeManager.FadeInFinishedFunc func;
        func = () =>
        {
            //アプリケーションを落とす
            Application.Quit();
        };

        //フェード開始
        CoroutineManager.Instance.StartCoroutineMethod(FadeManager.Instance.FadeInCoroutine(fadeFrame, func));
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

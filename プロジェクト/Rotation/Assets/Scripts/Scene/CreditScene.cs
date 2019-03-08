using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クレジットシーン
/// </summary>
public class CreditScene : IScene {

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //  BGM再生(ループなし)
        float frame = SceneController.Instance.FadeFrame;
        SoundManager.Instance.StartSoundFadeOut(frame);
        SoundManager.Instance.Play("ending");
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        Input();
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

    /// <summary>
    /// 入力受付
    /// </summary>
    private void Input()
    {
        var input = MyInputManager.AllController;

        //  STARTボタンでシーン遷移を開始
        if (input.START)
        {
            //効果音
            SoundManager.Instance.PlayOnSE("se");

            //タイトルに戻る処理
            ReturnTitle();
        }
    }

    /// <summary>
    /// タイトルに戻る関数
    /// </summary>
    private void ReturnTitle()
    {
        //  シーン遷移
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.TITLE);

        //  音フェード
        float frame = SceneController.Instance.FadeFrame;
        SoundManager.Instance.StartSoundFadeIn(frame);
    }
}

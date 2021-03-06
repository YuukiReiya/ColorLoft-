﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン管理用クラス
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController> {

    [SerializeField,] int fadeFrame = 60;
    public int FadeFrame { get { return fadeFrame; } }

    IScene root;

    //  const param!
    const int SingletonSceneIndex = 1;//シングルトンシーンのビルドインデックス

    /// <summary>
    /// シーン列挙体
    /// ※SceneのbuildIndexと同じ値にする
    /// </summary>
    public enum SCENE
    {
        TITLE = 2,  //タイトル
        GAME,       //ゲーム
        RESULT,     //リザルト
        ENTRY,      //エントリー
        SETTING,    //ゲーム設定
        CREDIT,     //クレジット
    }

    /// <summary>
    /// シーンの初期化
    /// </summary>
    public void Initialize()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        root = ConvertScene((SCENE)index);
        root.Start();
    }

    /// <summary>
    /// シーンの更新処理
    /// </summary>
    public void SceneUpdate()
    {

        //  フェード中は更新しない
        if (!FadeManager.Instance.IsFade)
        {
            root.Update();
        }
    }

    /// <summary>
    /// シーンの紐づけを行う
    /// </summary>
    /// <param name="eScene"></param>
    /// <returns>変換先シーン</returns>
    private IScene ConvertScene(SCENE eScene)
    {

        switch (eScene)
        {
            case SCENE.GAME:return new GameScene();
            case SCENE.TITLE:return new TitleScene();
            case SCENE.RESULT: return new ResultScene();
            case SCENE.ENTRY: return new EntryScene();
            //case SCENE.SETTING:return new SettingScene();
            case SCENE.CREDIT:return new CreditScene();
        }

        Debug.LogError("<color=red>this is not convert scene!</color>");
        return null;
    }

    /// <summary>
    /// シーンの紐づけを行う
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    private SCENE ConvertScene(IScene scene)
    {
        if(scene is GameScene) { return SCENE.GAME; }
        else if(scene is TitleScene) { return SCENE.TITLE; }
        else if(scene is ResultScene) { return SCENE.RESULT; }
        else if(scene is EntryScene) { return SCENE.ENTRY; }
        //else if(scene is SettingScene) { return SCENE.SETTING; }
        else if(scene is CreditScene) { return SCENE.CREDIT; }
        Debug.LogError("<color=red>this is not convert scene!</color>");
        return 0;
    }

    /// <summary>
    /// フェードを使ったシーン遷移
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="fadeFrame"></param>
    public void LoadFadeScene(SCENE scene, FadeManager.FadeInFinishedFunc func = null)
    {
        //コルーチンの登録
        IEnumerator wait = WaitForSceneLoaded(ConvertScene(scene));
        //匿名関数をコルーチンの引数に
        IEnumerator coroutine = FadeManager.Instance.SceneFadeCoroutine(
            fadeFrame,
            () =>
                {
                    SceneManager.LoadScene((int)scene);
                    StartCoroutine(wait);
                    //Scene add = SceneManager.GetSceneAt(SingletonSceneIndex);
                    //SceneManager.SetActiveScene(add);//シングルトンシーンの追加
                }
            );
        //コルーチンの開始
        StartCoroutine(coroutine);
    }

    /// <summary>
    /// シーンが遷移するまで待機
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForSceneLoaded(IScene scene)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == (int)ConvertScene(scene));
        root = scene;
        scene.Start();
    }
}

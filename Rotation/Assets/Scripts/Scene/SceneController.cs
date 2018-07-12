using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン管理用クラス
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController> {

    [SerializeField,] int fadeFrame = 60;

    IScene root;

    /// <summary>
    /// シーン列挙体
    /// ※Sceneのインデックスと同じ値にする
    /// </summary>
    public enum SCENE
    {
        GAME = 1,
        TITLE,
        RESULT,
        ENTRY,
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

    // Use this for initialization
    void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
		
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
                    Scene add = SceneManager.GetSceneAt(0);//シングルトンシーンの追加
                    SceneManager.SetActiveScene(add);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン管理用クラス
/// </summary>
public class SceneController : SingletonMonoBehaviour<SceneController> {


    IScene root;

    /// <summary>
    /// シーン列挙体
    /// </summary>
    public enum SCENE
    {
        GAME = 1,
        TITLE,
        RESULT,
    }

    /// <summary>
    /// シーンの初期化
    /// </summary>
    public void Initialize()
    {
        root = ConvertScene(SCENE.GAME);
        root.Start();
    }

    /// <summary>
    /// シーンの更新処理
    /// </summary>
    public void SceneUpdate()
    {
        root.Update();
    }

    // Use this for initialization
    void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// シーンの変換を行う
    /// </summary>
    /// <param name="eScene"></param>
    /// <returns>変換先シーン</returns>
    private IScene ConvertScene(SCENE eScene)
    {

        switch (eScene)
        {
            case SCENE.GAME:return new GameScene();
            case SCENE.TITLE:return new TitleScene();
        }

        Debug.LogError("<color=red>this is not convert scene!</color>");
        return null;
    }
}

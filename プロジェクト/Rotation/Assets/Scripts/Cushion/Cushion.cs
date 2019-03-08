using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クッション用のシーンの処理
/// </summary>
public class Cushion : MonoBehaviour {

	// Use this for initialization
	void Start () {

        FadeManager.FadeInFinishedFunc func = () =>
        {
            SceneController.Instance.Initialize();
        };
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.TITLE, func);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

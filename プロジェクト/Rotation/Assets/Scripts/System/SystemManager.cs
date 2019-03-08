using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// システム進行管理
/// </summary>
public class SystemManager : SingletonMonoBehaviour<SystemManager> {

    //  private param!
    [SerializeField, Tooltip("ゲームのTargetFrameRate")] int fps = 60;

#if UNITY_EDITOR
    [SerializeField,Tooltip("Cushionシーンの動作確認用フラグ(他シーンから入る場合入れる)")] bool isDebug = true;
#endif
    //  Property!
    public int Fps { get { return fps; } }

    //  Awake!
    private void Awake()
    {
        Application.targetFrameRate = fps;
    }

    // Use this for initialization
    void Start () {

        //  clock zero!
        WorldClock.CountZero();

        //  fade initialize!
        FadeManager.Instance.Initialize();

#if UNITY_EDITOR
        if(isDebug)
        //  scene initialize!
        SceneController.Instance.Initialize();
#endif
        //  Don't Destroy!
        DontDestroyOnLoad(Instance.gameObject);
        DontDestroyOnLoad(FadeManager.Instance.gameObject);
        DontDestroyOnLoad(SceneController.Instance.gameObject);
        DontDestroyOnLoad(PlayerManager.Instance.gameObject);
        DontDestroyOnLoad(CoroutineManager.Instance.gameObject);
        DontDestroyOnLoad(MyInputManager.AllController.gameObject);//all input object
    }
	
	// Update is called once per frame
	void Update () {

        //  clock count!
        WorldClock.CountUp();

        //  scene update!
        SceneController.Instance.SceneUpdate();
	}
}

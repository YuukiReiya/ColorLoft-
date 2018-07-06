using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// システム進行管理
/// </summary>
public class SystemManager : SingletonMonoBehaviour<SystemManager> {

    //  private param!
    [SerializeField, Tooltip("ゲームのTargetFrameRate")] int fps = 60;

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

        //  scene initialize!
        SceneController.Instance.Initialize();

        //  Don't Destroy!
        DontDestroyOnLoad(Instance.gameObject);
        DontDestroyOnLoad(FadeManager.Instance.gameObject);
        DontDestroyOnLoad(SceneController.Instance.gameObject);

    }
	
	// Update is called once per frame
	void Update () {

        //  clock count!
        WorldClock.CountUp();

        //  scene update!
        SceneController.Instance.SceneUpdate();
	}
}

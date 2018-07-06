using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 制限時間
/// </summary>
public class TimeLimit : SingletonMonoBehaviour<TimeLimit> {


    //  private param!
    [SerializeField, Tooltip("1ゲームの制限時間(秒)")] int sec;
    [SerializeField] Text text;

    private int count;
    private int minute { get { return count / SystemManager.Instance.Fps; } }
    private int second { get { return count % SystemManager.Instance.Fps; } }
    private int f = 0;

    //  public param!
    public bool isTimeLimit { get { return count == 0; } }

	// Use this for initialization
	void Start () {
        count = sec;
        text.text = (minute / 10).ToString() + (minute % 10).ToString() + ":" + (second / 10).ToString() + (second % 10).ToString();
        f = 0;
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    public void CountUp()
    {
        text.text = (minute / 10).ToString() + (minute % 10).ToString() + ":" + (second / 10).ToString() + (second % 10).ToString();

        //  一秒ずつ減算
        if (++f == SystemManager.Instance.Fps)
        {
            f = 0;
            count--;
            if (count < 0) { count = 0; }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 制限時間
/// </summary>
public class TimeLimit : SingletonMonoBehaviour<TimeLimit> {

    //  private param!
    [SerializeField, Tooltip("1ゲームの制限時間(秒)")] int sec;
    [SerializeField] TextMeshProUGUI text;

    private int count;
    private int minute { get { return count / SystemManager.Instance.Fps; } }
    private int second { get { return count % SystemManager.Instance.Fps; } }
    private int frame = 0;

    //  public param!
    public bool isTimeLimit { get { return count == isGameEndCount; } }

    //  const param!
    const int isGameEndCount = 0;  //  カウントが"0"の状態を表示してから終了させたいので

	// Use this for initialization
	void Start () {
        count = sec;
        text.text = (minute / 10).ToString() + (minute % 10).ToString() + ":" + (second / 10).ToString() + (second % 10).ToString();
        frame = 0;
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    /// <summary>
    /// タイマー処理
    /// </summary>
    public void CountUp()
    {
        text.text = (minute / 10).ToString() + (minute % 10).ToString() + ":" + (second / 10).ToString() + (second % 10).ToString();

        //  一秒ずつ減算
        if (++frame == SystemManager.Instance.Fps)
        {
            frame = 0;
            count--;
            if (count < isGameEndCount) { count = isGameEndCount; }
        }
    }

    /// <summary>
    /// タイマーのセット
    /// </summary>
    /// <param name="time"></param>
    public void SetTimeLimit(DataBase.TimeLimit time = DataBase.TimeLimit.TREE_MINUTE)
    {
        sec = (int)time;
        count = sec;
        text.text = (minute / 10).ToString() + (minute % 10).ToString() + ":" + (second / 10).ToString() + (second % 10).ToString();
    }
}

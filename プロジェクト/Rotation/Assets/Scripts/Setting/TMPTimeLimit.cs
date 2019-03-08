using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// TMPを使った制限時間の表記
/// </summary>
public class TMPTimeLimit : MonoBehaviour {

    [Header("Parameter")]
    [SerializeField, Tooltip("分 を入れるTMPGUI")] TextMeshProUGUI minuteNumber;
    [SerializeField, Tooltip("秒 を入れるTMPGUI")] TextMeshProUGUI secondNumber;

    /// <summary>
    /// 制限時間に応じた時間をテキストに設定する
    /// </summary>
    /// <param name="time"></param>
    public void SetTimeTexts(DataBase.TimeLimit time)
    {
        //  int型にキャスト
        int iTime = (int)time;
        int oneMin= (int)DataBase.TimeLimit.ONE_MINUTE;

        Debug.Log("iTime = " + iTime + "\noneMin = " + oneMin);

        //  〇分の〇を求める
        int iMinutes = iTime / oneMin;

        //  0 か 30 のどちらなのかを判断する
        bool isHarf = iTime % oneMin != 0;

        //代入
        minuteNumber.text = iMinutes.ToString();
        secondNumber.text = isHarf ? "30" : "00";
    }

}

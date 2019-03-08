using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// TMP
/// </summary>
public class TMPGameMode : MonoBehaviour {

    [SerializeField] TextMeshProUGUI survivalTexts;
    [SerializeField] TextMeshProUGUI pointBattleTexts;

    /// <summary>
    /// モードの設定
    /// </summary>
    /// <param name="mode"></param>
    public void SetMode(DataBase.MODE mode)
    {
        switch (mode)
        {
            case DataBase.MODE.SCORE_SYSTEM:
                survivalTexts.gameObject.SetActive(false);
                pointBattleTexts.gameObject.SetActive(true);
                return;
            case DataBase.MODE.STOCK_SYSTEM:
                survivalTexts.gameObject.SetActive(true);
                pointBattleTexts.gameObject.SetActive(false);
                return;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エントリーシーンのタイトルへ戻るためのUI
/// </summary>
public class ReturnTitleUI : SingletonMonoBehaviour<ReturnTitleUI> {


    [SerializeField] UnityEngine.UI.Image accumulationImage;

    public void SetFillAmountValue(float fillValue)
    {
        accumulationImage.fillAmount = fillValue;
    }


}

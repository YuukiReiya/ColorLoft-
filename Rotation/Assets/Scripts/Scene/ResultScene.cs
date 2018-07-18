using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// リザルトシーン
/// </summary>
public class ResultScene : IScene {

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //PlayerManager.Instance.Set();
        PlayerManager.Instance.GetPlayer(1).StartWinAnimation();
        PlayerManager.Instance.GetPlayer(0).StartLoseAnimation();
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {

    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

}

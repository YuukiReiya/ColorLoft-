﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームシーン
/// </summary>
public class GameScene :IScene {

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {

    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        //  制限時間が0ならゲーム終了
        if (TimeLimit.Instance.isTimeLimit) { return; }

        //  プレイヤー更新
        PlayerManager.Instance.PlayersUpdate();

    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

}

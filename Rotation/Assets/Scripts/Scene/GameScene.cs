using System.Collections;
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
        // PlayerManager.Instance.Set();
        PlayerManager.Instance.CreatePlayer();

        GameStartCount.Instance.CountUp();
    }

    bool f = false;
    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {

        if (!GameStartCount.Instance.isStart) { return; }

        TimeLimit.Instance.CountUp();

        //  制限時間が0ならシーン遷移
        if (TimeLimit.Instance.isTimeLimit) {

            if (f) { return; }
            SceneController.Instance.LoadFadeScene(SceneController.SCENE.RESULT);
            f = true;
        }

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

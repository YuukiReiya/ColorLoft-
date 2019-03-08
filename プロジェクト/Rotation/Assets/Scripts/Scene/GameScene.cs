using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ゲームシーン
/// </summary>
public class GameScene :IScene {


    //  private param!
    IEnumerator finishCoroutine;

    //  const param!
    const float FINISH_WAIT_FRAME = 80;//ゲーム終了時の待機フレーム(SEとの同期)

    //  property
    bool isGameEnd
    {
        get
        {
            //制限時間が無くなった
            return TimeLimit.Instance.isTimeLimit;

        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        //初期化
        StageManager.Instance.Initialize();

        //生成＆参照セット
        PlayerManager.Instance.CreatePlayer();

        //参照を代入
        Player[] entryPlayer = new Player[DataBase.PLAYER_NUM];
        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            entryPlayer = PlayerManager.Instance.GetPlayers();
        }
        StageManager.Instance.SetPlayers(entryPlayer);

        //エフェクト
        HitEffectsPool.Instance.Initialize();

        //ゲーム終了時のコルーチン
        finishCoroutine = null;

        //ゲームルールの設定
        SetGameRule();

        //カウントアップ
        GameStartCount.Instance.CountUp();

    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        //カウント終了まで待機
        if (!GameStartCount.Instance.isStart) { return; }

        TimeLimit.Instance.CountUp();

        //  制限時間が0ならシーン遷移
        if (isGameEnd) {
            GameFinish();
            return;//プレイヤーに処理させない
        }

        //  プレイヤー更新
        PlayerManager.Instance.PlayersUpdate();

        //  ステージ更新
        StageManager.Instance.StageUpdate();

    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Destroy()
    {

    }

    /// <summary>
    /// ゲームルールの設定
    /// (ゲームモード，制限時間，etc)
    /// </summary>
    private void SetGameRule()
    {
        //制限時間の設定
        TimeLimit.Instance.SetTimeLimit(EntryScene.PlayTime);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    private void GameFinish()
    {
        //既に動いていたら処理しない
        if (finishCoroutine != null) { return; }
        finishCoroutine = GameFinishCoroutine();
        CoroutineManager.Instance.StartCoroutineMethod(finishCoroutine);
    }

    /// <summary>
    /// ゲーム終了のコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameFinishCoroutine()
    {
        //効果音
        SoundManager.Instance.PlayOnSE("GameFinish");

        //待機コルーチン
        for(int i = 0; i < FINISH_WAIT_FRAME; ++i) { yield return null; }

        //シーン遷移
        SceneController.Instance.LoadFadeScene(SceneController.SCENE.RESULT);
    }
}

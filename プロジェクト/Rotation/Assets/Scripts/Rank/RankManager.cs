using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

/// <summary>
/// プレイヤーのランク付けを管理
/// </summary>
public class RankManager : SingletonMonoBehaviour<RankManager> {

    //  serialize param!
    [SerializeField,Tooltip("キャラクターの獲得したポイントの表示をするテキスト")] TextMeshProUGUI[] getPointTexts;

    //  private param!
    //Player[]    players;    //保存用変数
    //int[]       ranks;      //添え字が小さい順に順位の数値が入っている

#if UNITY_EDITOR
    [SerializeField] bool isRankDebug = false;
    [SerializeField] int _1pPoint;
    [SerializeField] int _2pPoint;
    [SerializeField] int _3pPoint;
    [SerializeField] int _4pPoint;
#endif
    struct RankData
    {
        public int index;
        public int point;
        public int rank;
    }

    RankData[] ranks;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        ranks = new RankData[DataBase.PLAYER_NUM];

        var pm = PlayerManager.Instance;
        MakeRank(pm.GetPlayers());      //順位付け
    }

    public void DrawDebug()
    {
        //var players = PlayerManager.Instance.GetPlayers();
        //for (int i = 0; i < players.Length; ++i)
        //{
        //    if(players[i])
        //    Debug.Log("コントローラー番号" + players[i].GetContoroller() + "/順位" + ranks[i] + "\nポイント=" + players[i].Point);
        //}
    }

    /// <summary>
    /// 配列の前後の獲得ポイントの比較を行い順位を割り振る
    /// ※スコアの高い順に並び替えられてることが前提
    /// </summary>
    /// <param name="list"></param>
    private void MakeRank(Player[] list)
    {
        //  ランク付けに必要ばデータの用意
        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            //初期化
            ranks[i].index = i;
            ranks[i].rank = 1;
            ranks[i].point = 0;
        }

        //  参加プレイヤーのポイントの取得
        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            //参加プレイヤーのポイントのみ取得
            int id = PlayerManager.Instance.GetID(i);
            if (id == PlayerManager.NOT_ENTRY) { continue; }
            ranks[i].point = PlayerManager.Instance.GetPlayer(i).Point;
        }

        //////////////////////////////////////////////
        //  デバッグ
        //////////////////////////////////////////////

#if UNITY_EDITOR
        if (isRankDebug)
        {
            ranks[0].point = _1pPoint;
            ranks[1].point = _2pPoint;
            ranks[2].point = _3pPoint;
            ranks[3].point = _4pPoint;
        }
#endif
        //////////////////////////////////////////////

        //  データを獲得ポイントの降順にソート
        ranks = ranks.OrderByDescending(it => it.point).ToArray();

        //  ローカル変数
        int rank = 1;       //順位付けする順位
        int prevIndex = -99;//ひとつ前のインデックス
        int prevScore = -99;//ひとつ前の獲得ポイント
        int index = 0;      //現在参照中のインデックス

        foreach(var it in ranks)
        {
            int score = it.point;

            //  ランク付け
            if (score != prevScore)
            {
                ranks[index].rank = rank++;
            }
            //  同順処理
            else
            {
                ranks[index].rank = ranks[prevIndex].rank;
            }

            //  ひとつ前の情報を更新
            prevIndex = index;
            prevScore = score;
            index++;
        }

        //  インデックスの昇順にソート
        ranks = ranks.OrderBy(it => it.index).ToArray();

#if UNITY_EDITOR

        if (isRankDebug)
        {
            Debug.Log("1P = " + ranks[0].rank + "位");
            Debug.Log("2P = " + ranks[1].rank + "位");
            Debug.Log("3P = " + ranks[2].rank + "位");
            Debug.Log("4P = " + ranks[3].rank + "位");
        }

#endif
    }

    /// <summary>
    /// リザルト時のアニメーション開始命令
    /// </summary>
    public void StartResultAnimation()
    {
        //ランク格納したを配列を用意
        int[] ranks = new int[this.ranks.Length];
        for (int i = 0; i < ranks.Length; ++i)
        {
            int index = this.ranks[i].index;
            ranks[index] = this.ranks[index].rank;
        }

        ViewerManager.Instance.RankOfAnimation(ranks);
    }

    /// <summary>
    /// プレイヤーの獲得したポイントをテキストへ反映
    /// </summary>
    public void SetPoint()
    {
        //テキストへ代入
        for(int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            var pm = PlayerManager.Instance;
            int id = pm.GetID(i);

            //非参加プレイヤーなら飛ばす
            if (id == PlayerManager.NOT_ENTRY) continue;

            //インデックス算出
            int index = DataBase.GetControllerID(id) - 1;

            //プレイヤーの取得
            var player = pm.GetPlayer(index);

            getPointTexts[index].text = player.Point.ToString();
        }
    }
}

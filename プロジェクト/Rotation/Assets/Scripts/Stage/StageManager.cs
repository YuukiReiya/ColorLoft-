using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ管理のマネージャー
/// </summary>
public class StageManager : SingletonMonoBehaviour<StageManager> {

    [System.Serializable]
    public struct BlockColor
    {
        public Color normalColor;
        public Color blinkerColor;
    }

    //  serialize param!
    [SerializeField] BlockColor _1pBlockColor;
    [SerializeField] BlockColor _2pBlockColor;
    [SerializeField] BlockColor _3pBlockColor;
    [SerializeField] BlockColor _4pBlockColor;

    //  private param!
    private List<Block> blocks;
    private bool allChild = false;//非アクティブな子オブジェクトもGetComponentsInChildrenの対象にするか
    private GameObject[] childObjects;
    private Player[] players;

    //  const param!
    private const float BLOCK_FALL_SE_VOLUME = 0.5f;//ブロック落下時のSEの音量

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        blocks = new List<Block>();

        var tmp = GetComponentsInChildren<Block>(allChild);

        foreach (var it in tmp)
        {
            blocks.Add(it);
        }
    }

    /// <summary>
    /// プレイヤーのセッター
    /// </summary>
    /// <param name="players"></param>
    public void SetPlayers(Player[] players)
    {
        this.players = players;

        childObjects = new GameObject[this.players.Length];
        for (var i = 0; i < childObjects.Length; ++i)
        {
            //生成されてないものは飛ばす
            if (!players[i]) continue;

            childObjects[i] = new GameObject();

            var obj = players[i].transform.GetComponentInChildren<Animator>().gameObject;

            //Animatorコンポーネントがついている子オブジェクト(=移動対象モデル)の取得
            childObjects[i] = obj;
        }
    }

    /// <summary>
    /// ステージの更新処理
    /// </summary>
    public void StageUpdate()
    {
        for(int i = 0; i < players.Length; ++i)
        {
            //生成されてないプレイヤーは飛ばす
            if (!players[i]) continue;

            foreach (var it in blocks)
            {
                //  プレイヤーの座標を確認
                Vector3 modelPos = childObjects[i].transform.position;

                //  プレイヤーがプレイヤー色のブロックに乗っている？
                bool ret = it.CheckColorArea(modelPos, players[i].color);

                //  一度踏まれている？
                bool isFall = it.GetIsFall(players[i].color);

                //  乗っていない状態 かつ 一度踏まれている かつ ブロックが点滅していない
                if (!ret && isFall)
                {
                    //  落下を開始
                    it.StartFall(players[i].color);

                    //  効果音
                    SoundManager.Instance.PlayOnSE("BlockFall", BLOCK_FALL_SE_VOLUME);

                    //  フラグのリセット
                    it.ResetIsFall(players[i].color);
                }
            }

        }
    }

    /// <summary>
    /// ブロックの色を返却
    /// </summary>
    /// <returns></returns>
    public BlockColor? GetBlockColor(DataBase.COLOR color)
    {
        BlockColor? ret = null;

        switch (color)
        {
            case DataBase.COLOR.RED:
                ret = _1pBlockColor;
                break;
            case DataBase.COLOR.BLUE:
                ret = _2pBlockColor;
                break;
            case DataBase.COLOR.GREEN:
                ret = _3pBlockColor;
                break;
            case DataBase.COLOR.YELLOW:
                ret = _4pBlockColor;
                break;
            default:
                break;
        }

        return ret;
    }
}

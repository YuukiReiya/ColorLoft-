using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージ管理のマネージャー
/// </summary>
public class StageManager : SingletonMonoBehaviour<StageManager> {

    List<Block> blocks;


    private bool allChild = true;//非アクティブな子オブジェクトもGetComponentsInChildrenの対象にするか



    [SerializeField,] Player[] players;

    // Use this for initialization
    void Start () {

        blocks = new List<Block>();

        var tmp = GetComponentsInChildren<Block>(allChild);


        foreach(var it in tmp)
        {
            blocks.Add(it);
        }
	}
	
	// Update is called once per frame
	void Update () {


        foreach(Player player in players)
        {
            //  落下判定
            foreach (var it in blocks)
            {
                Vector3 modelPos = player.transform.GetComponentInChildren<Animator>().gameObject.transform.position;

                //  プレイヤーがプレイヤー色のブロックに乗っている？
                bool ret = it.CheckColorArea(modelPos, player.color);

                //  一度踏まれている？
                bool isFall = it.GetIsFall(player.color);

                //  乗っていない状態かつ一度踏まれている
                if (!ret && isFall)
                {
                    //  落下を開始
                    it.StartFall(player.color);
                    
                    //  フラグのリセット
                    it.ResetIsFall(player.color);
                }
            }
        }
	}
}

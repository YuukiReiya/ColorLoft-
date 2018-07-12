using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :SingletonMonoBehaviour<PlayerManager> {

    [SerializeField,] Player[] playersPrefab;

    Player[] players;
    int[] id;


    // Use this for initialization
    void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 更新処理
    /// </summary>
    public void PlayersUpdate()
    {
        foreach(var it in players)
        {
            it.PlayerUpdate();
        }
    }


    void CreatePlayer()
    {
        players = new Player[2];
    }

    /// <summary>
    /// プレイヤーの取得
    /// </summary>
    /// <param name="index">番号</param>
    /// <returns>取得したプレイヤー</returns>
    public Player GetPlayer(int index)
    {
        Player p = null;

        //下限をオーバー
        if (index < 0) { p = players[0]; }
        //上限をオーバー
        else if (players.Length <= index) { p = players[(players.Length - 1)]; }
        //エラー無し
        else  { p = players[index]; }

        return p;
    }

    /// <summary>
    /// 仮処理
    /// </summary>
    public void Set()
    {


        var tmp = Object.FindObjectsOfType<Player>();

        players = new Player[tmp.Length];

        int index = 0;
        foreach (var it in tmp)
        {
            players[index] = it;
            index++;
        }

    }

}

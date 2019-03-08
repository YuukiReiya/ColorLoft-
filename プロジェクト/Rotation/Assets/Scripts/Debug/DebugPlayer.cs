using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayer : MonoBehaviour {

    Player[] players;

	// Use this for initialization
	void Start () {

        var tmp = Object.FindObjectsOfType<Player>();

        players = new Player[tmp.Length];
        int index = 0;
        foreach (var it in tmp)
        {
            players[index] = it;
            players[index].SetColor(DataBase.COLOR.RED);
            players[index].SetControllerIndex((GamePadInput.GamePad.Index)index + 1);
            players[index].SetPlayerModel(it.gameObject);
            index++;
        }

        foreach(var it in players)
        {
            it.DebugInit();
        }


    }

    // Update is called once per frame
    void Update () {
		

        foreach(var it in players)
        {
            it.PlayerUpdate();
        }
	}
}

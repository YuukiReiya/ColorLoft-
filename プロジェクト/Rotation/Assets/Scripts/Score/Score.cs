using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {

    [SerializeField] int index;
    [SerializeField] TextMeshProUGUI text;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {


        string str = string.Empty;

        var player = PlayerManager.Instance.GetPlayer(index);

        if (!player) { return; }
        str = player.Point.ToString();
        text.text = str;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコア
/// </summary>
public class Score : MonoBehaviour {

    [SerializeField] Player player;

    Text text;

	// Use this for initialization
	void Start () {

        text = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {

        text.text = player.GetPoint().ToString();

	}
}

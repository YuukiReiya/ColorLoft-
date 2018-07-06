using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの上のガイド表示
/// </summary>
public class GuideArrow : MonoBehaviour {



    //  
    GameObject obj;

	// Use this for initialization
	void Start () {

        obj = this.gameObject;

	}
	
	// Update is called once per frame
	void Update () {

        obj.transform.LookAt(Camera.main.transform);

	}
}

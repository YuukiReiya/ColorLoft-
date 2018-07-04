using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの上のガイド表示
/// </summary>
public class GuideArrow : MonoBehaviour {


    //  
    [SerializeField, Tooltip("表示するスプライト")] private Sprite sprite;
    [SerializeField] Image image;

    [SerializeField] GameObject target;

    [SerializeField] Vector2 offset;

    //  

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 pos = target.transform.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        screenPos.x += offset.x;
        screenPos.y += offset.y;

        image.rectTransform.position = screenPos;

	}
}

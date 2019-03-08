using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルロゴに関するスクリプト
/// </summary>
public class TitleLogo : MonoBehaviour {

    [SerializeField] Image ui;
    [SerializeField] SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        Set();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Set()
    {
        //UI座標からスクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, ui.rectTransform.position);

        //ワールド座標
        Vector3 worldPos = Vector3.zero;

        //スクリーン座標→ワールド座標に変換
        RectTransformUtility.ScreenPointToWorldPointInRectangle(ui.rectTransform, screenPos, Camera.main, out worldPos);

        //座標セット
        sprite.transform.position = worldPos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeImage : MonoBehaviour {

    //private param!
    [SerializeField, Tooltip("0:SCORE,1:STOCK")] private Sprite[] sprites;
    private Image image;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        image = GetComponent<Image>();
        SetSprite(0);
    }

    /// <summary>
    /// スプライトセッター
    /// 引数には配列の添え字
    /// </summary>
    /// <param name="index"></param>
    public void SetSprite(int index)
    {
        image.sprite = sprites[index];
    }
}

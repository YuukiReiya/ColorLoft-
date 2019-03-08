using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ポイント増減時のUIのプール
/// </summary>
public class PointUIPools : SingletonObjectPoolBase<PointUIPools> {

    [SerializeField] float floatFrame = 60;

    [SerializeField] Color _1pColor;
    [SerializeField] Color _2pColor;
    [SerializeField] Color _3pColor;
    [SerializeField] Color _4pColor;

    //  const param!
    const float UI_ALPHA_MAX = 1.0f;
    const float UI_ALPHA_MIN = 0.0f;
    const float UI_MOVE_WEIGHT = 2.0f;//UIを移動する際の重み

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 加点表示
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pos"></param>
    /// <param name="point"></param>
    public void DrawPositiveScore(GamePadInput.GamePad.Index index, Vector3 pos, int point)
    {
        var obj = GetObject();

        //uguiがnullならPrefabにTextMeshProUGUIが付いていない
        var ugui = obj.GetComponent<TextMeshProUGUI>();

        //スクリーン座標に変換
        Vector3 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        ugui.rectTransform.position = rectPos;

        //テキスト色
        ugui.color = ConvertPlayerColor(index);

        //表示文字列
        string str = "+" + point;

        //表示
        DrawScore(ugui, str);

        StartCoroutine(DrawFloatMoveCoroutine(ugui));
    }

    /// <summary>
    /// 減点表示
    /// </summary>
    /// <param name="index"></param>
    /// <param name="pos"></param>
    /// <param name="point"></param>
    public void DrawNegativeScore(GamePadInput.GamePad.Index index, Vector3 pos, int point)
    {
        var obj = GetObject();

        //uguiがnullならPrefabにTextMeshProUGUIが付いていない
        var ugui = obj.GetComponent<TextMeshProUGUI>();

        //スクリーン座標に変換
        Vector3 rectPos = RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        ugui.rectTransform.position = rectPos;

        //テキスト色
        ugui.color = ConvertPlayerColor(index);

        //表示文字列
        string str = "-" + point;

        //表示
        DrawScore(ugui, str);

        //フロートアウト
        StartCoroutine(DrawFloatMoveCoroutine(ugui));

    }

    void DrawScore(TextMeshProUGUI text, string score)
    {
        text.text = score;
    }

    /// <summary>
    /// フロートアウトしながら上方向へ移動するコルーチン
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    IEnumerator DrawFloatMoveCoroutine(TextMeshProUGUI text)
    {
        //α値の補正
        var cr = text.color;
        cr.a = UI_ALPHA_MAX;
        text.color = cr;

        //フロートアウト
        float value = UI_ALPHA_MAX / floatFrame;//変量

        for(int i = 0; i < floatFrame; ++i)
        {
            //α値の減算
            cr.a -= value;

            //方向ベクトルに加算
            text.rectTransform.position += Vector3.up * UI_MOVE_WEIGHT;

            //反映
            text.color = cr;

            yield return null;
        }



        //プールに戻す
        text.gameObject.SetActive(false);
    }

    /// <summary>
    /// プレイヤーに対応した色を返す
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    Color ConvertPlayerColor(GamePadInput.GamePad.Index index)
    {
        switch (index)
        {
            case GamePadInput.GamePad.Index.One:
                return _1pColor;
            case GamePadInput.GamePad.Index.Two:
                return _2pColor;
            case GamePadInput.GamePad.Index.Three:
                return _3pColor;
            case GamePadInput.GamePad.Index.Four:
                return _4pColor;
            default:
                break;
        }

        Debug.LogError(" <color=red>invalid value</color>");
        return new Color(0, 0, 0, 0);
    }
}

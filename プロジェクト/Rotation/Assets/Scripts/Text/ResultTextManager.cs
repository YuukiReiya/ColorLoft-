using System.Collections;
using System.Collections.Generic;
using MethodExpansion;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// リザルトシーンのテキストを管理
/// </summary
public class ResultTextManager : SingletonMonoBehaviour<ResultTextManager> {

    //  serialize param!
    [Header("Control Parameter")]
    [SerializeField, Tooltip("スケール変更するフレーム")] float scalingFrame = 10;
    [SerializeField] ResultTexts[] resultTexts;
    [System.Serializable]
    struct ResultTexts
    {
        public Image textBackGround;
        public TextMeshProUGUI resultText;
    }


    [Header("Text Contents Value")]
    [SerializeField, TextArea] string entryText = "スタートボタンを押してください";            //参加待ちテキスト
    [SerializeField, TextArea] string finishedText = "他の人を待っています";                   //準備完了テキスト
    [SerializeField, TextArea] string notEntryText = "参加していません";                       //非参加プレイヤー用テキスト

    //  private param!
    Vector3 initBackFramePos;
    Vector3 initTextPos;


    /// <summary>
    /// テキストのアクティブ切り替え
    /// </summary>
    /// <param name="flags"></param>
    private void ActivateText(int index, bool flags)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > resultTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        resultTexts[index].resultText.gameObject.SetActive(flags);
    }

    /// <summary>
    /// 縦方向にテキストのバックを閉じて開く
    /// </summary>
    public void StartCloseAndOpenVertical(int index, CoroutineExpansion.CoroutineFinishedFunc func = null)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > resultTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        //コルーチンの実行
        IEnumerator coroutine = CloseAndOpenVertical(index);
        this.StartCoroutine(coroutine, func);
    }

    /// <summary>
    /// 縦方向にテキストのバックを閉じて開くコルーチン
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IEnumerator CloseAndOpenVertical(int index)
    {
        //インデックスのチェックは呼び出し前に行うのでしない

        //テキスト非表示
        ActivateText(index, false);

        //参照
        var entryBackRef = resultTexts[index].textBackGround;

        //変更前の大きさ
        float maxScale = entryBackRef.rectTransform.localScale.y;

        //1フレーム辺りの大きさの変化量
        float scaleValue = maxScale / scalingFrame;

        //閉じる
        for (int i = 0; i < scalingFrame; ++i)
        {
            var scale = entryBackRef.rectTransform.localScale;
            scale.y -= scaleValue;
            entryBackRef.rectTransform.localScale = scale;

            yield return null;
        }

        //待機フレーム
        for (int i = 0; i < 0; ++i) yield return null;

        //テキスト表示
        ActivateText(index, true);

        //開く
        for (int i = 0; i < scalingFrame; ++i)
        {
            var scale = entryBackRef.rectTransform.localScale;
            scale.y += scaleValue;
            entryBackRef.rectTransform.localScale = scale;

            yield return null;
        }

        yield break;
    }

    /// <summary>
    /// 待機用テキストのセット
    /// </summary>
    /// <param name="index"></param>
    public void SetWaitText(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > resultTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        resultTexts[index].resultText.text = entryText;
    }

    /// <summary>
    /// 入力終了後のテキストのセット
    /// </summary>
    /// <param name="index"></param>
    public void SetFinishedText(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > resultTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        resultTexts[index].resultText.text = finishedText;
    }

    /// <summary>
    /// 非参加者用テキストのセット
    /// </summary>
    /// <param name="index"></param>
    public void SetNotEntryText(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > resultTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        resultTexts[index].resultText.text = notEntryText;
    }

}

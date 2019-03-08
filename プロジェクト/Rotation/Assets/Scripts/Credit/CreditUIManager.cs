using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MethodExpansion;

/// <summary>
/// クレジットシーン上のUIを管理
/// </summary>
public class CreditUIManager : SingletonMonoBehaviour<CreditUIManager> {

    //  serialize param!
    [Header("Credit Control Parameter")]
    [SerializeField, Tooltip("上へ移動するための空オブジェクト")] GameObject rollEmpty;
    [SerializeField, Tooltip("移動先座標")] float targetHeight;
    [SerializeField, Tooltip("移動速度")] float rollSpeed;
    [SerializeField, Tooltip("提供と開発環境の文字が消えていくフレーム")] float floatInFrame;
    [SerializeField, Tooltip("文字が消えるまで待機するフレーム")] float waitStartFrame;
    [SerializeField, Tooltip("文字が消えてから出現処理に入るまで待機するフレーム")] float waitBetweenFrame;
    [SerializeField, Tooltip("メッセージが出てくるフレーム")] float floatOutFrame;
    [SerializeField, Tooltip("提供テキスト")] TextMeshProUGUI offerText;
    [SerializeField, Tooltip("提供者名テキスト")] TextMeshProUGUI offerPersonText;
    [SerializeField, Tooltip("開発環境テキスト")] TextMeshProUGUI createToolText;
    [SerializeField, Tooltip("メッセージ")] TextMeshProUGUI messageText;

    //  const param!
    private const float MIN_ALPHA = 0;
    private const float MAX_ALPHA = 1;

    //  callback
    delegate void callback();

	// Use this for initialization
	void Start () {
        StartCoroutine(RollText());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// テキストを上のほうに巻いていく
    /// </summary>
    /// <returns></returns>
    IEnumerator RollText()
    {
        //  繰り返し
        while (true)
        {
            //  座標
            var pos = rollEmpty.transform.localPosition;

            //  高さの変更
            pos.y += rollSpeed;
            rollEmpty.transform.localPosition = pos;

            //  目的の高さに到達したらループを抜ける
            if (rollEmpty.transform.localPosition.y > targetHeight)
            {
                pos.y = targetHeight;
                rollEmpty.transform.localPosition = pos;
                break;
            }

            yield return null;
        }

        //  巻き終わってから実行する関数
        this.DelayMethod(EndRollFunc, waitStartFrame);
    }

    /// <summary>
    /// 巻き終わってから実行する関数
    /// </summary>
    private void EndRollFunc()
    {
        //  下記コルーチンが終了時に呼び出される関数
        CoroutineExpansion.CoroutineFinishedFunc endFunc = () =>
        {
            //  メッセージテキストのフロートアウト
            this.StartDelayCoroutine(FloatOutTexts(), waitBetweenFrame);
        };

        //  提供と開発環境テキストのフロートイン
        this.StartCoroutine(FloatInTexts(), endFunc);
    }
    /// <summary>
    /// 提供と開発環境テキストのフロートイン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FloatInTexts()
    {
        //  アルファ値の増減量
        float speed = MAX_ALPHA / floatInFrame;

        //  ループ
        while (true)
        {
            //提供
            var ofcr = offerText.color;
            ofcr.a -= speed;
            offerText.color = ofcr;

            //提供
            var ofpcr = offerPersonText.color;
            ofpcr.a -= speed;
            offerPersonText.color = ofpcr;

            //開発環境
            var ctcr = createToolText.color;
            ctcr.a -= speed;
            createToolText.color = ctcr;

            //見えなくなったら処理を抜ける
            if (ofcr.a <= MIN_ALPHA || ctcr.a <= MIN_ALPHA) { break; }

            yield return null;
        }
    }

    /// <summary>
    /// メッセージテキストのフロートアウト
    /// </summary>
    /// <returns></returns>
    private IEnumerator FloatOutTexts()
    {
        //  アルファ値の増減量
        float speed = MAX_ALPHA / floatOutFrame;

        //  ループ
        while (true)
        {
            //メッセージテキストの色取得
            var mtcr = messageText.color;
            mtcr.a += speed;
            messageText.color = mtcr;

            //完全に表示しきったら処理を抜ける
            if (mtcr.a > MAX_ALPHA) { break; }

            yield return null;
        }

    }
}

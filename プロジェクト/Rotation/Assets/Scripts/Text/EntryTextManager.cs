using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MethodExpansion;

/// <summary>
/// エントリーシーンのテキストを管理
/// </summary>
public class EntryTextManager : SingletonMonoBehaviour<EntryTextManager> {

    //  serialize param!
    [Header("Entry text Control Parameter")]
    [SerializeField, Tooltip("シュッて出るのに使うフレーム")] float readyGoMoveFrame = 15;
    [SerializeField] Vector3 initBackFramePos;
    [SerializeField] Vector3 initTextPos;
    [SerializeField, Tooltip("矢印の移動フレーム")] float arrowMoveFrame = 15;
    [SerializeField, Tooltip("矢印の移動量")] float arrowMovePower = 0.1f;

    [Header("Player Text Control Parameter")]
    [SerializeField, Tooltip("スケール変更するフレーム")] float scalingFrame = 10;
    [SerializeField] Image readyGoBackFrame;
    [SerializeField] GameObject readyGoTextObject;
    [SerializeField] EntryTexts[] entryTexts;
    [System.Serializable]
    struct EntryTexts
    {
        public Image textBackGround;
        public TextMeshProUGUI entryText;
        public Image nameBackGround;
        public TextMeshProUGUI nameText;
        public Image characterLeftArrowImage;
        public Image characterRightArrowImage;
        public Vector3 initLeftArrowPos;
        public Vector3 initRightArrowPos;
        public IEnumerator coroutine;//
    }

    [Header("Text Contents Value")]
    [SerializeField, TextArea] string entryText = "スタートボタンを押してください";            //参加待ちテキスト
    [SerializeField, TextArea] string selectCharaText = "キャラクターを選択してください";      //キャラ選択テキスト
    [SerializeField, TextArea] string readyText = "準備完了！";                                //準備完了テキスト
    [SerializeField, TextArea] string noEntryNameText = "No Entry";
    [SerializeField, TextArea] string SKohakuName = "こはくちゃん(夏)";                        //こはくちゃん(夏)
    [SerializeField, TextArea] string WKohakuName = "こはくちゃん(冬)";                        //こはくちゃん(冬)
    [SerializeField, TextArea] string SMisakiName = "みさきちゃん(夏)";                        //みさきちゃん(夏)
    [SerializeField, TextArea] string WMisakiName = "みさきちゃん(冬)";                        //みさきちゃん(冬)
    [SerializeField, TextArea] string SYukoName = "ゆうこちゃん(夏)";                          //ゆうこちゃん(夏)
    [SerializeField, TextArea] string WYukoName = "ゆうこちゃん(冬)";                          //ゆうこちゃん(冬)

    [Header("Time Text Control Parameter")]
    [SerializeField, ] TextMeshProUGUI timeLimitValueText;  //制限時間の実際に値を変えるテキスト

    //  private param!
    IEnumerator arrivalReadyGoCoroutine;//"準備完了"テキストの表示をキャンセルするための変数

    /// <summary>
    /// 制限時間を変更
    /// </summary>
    /// <param name="time"></param>
    public void SetTimeLimit(DataBase.TimeLimit time)
    {
        string str = string.Empty;

        switch (time)
        {
            case DataBase.TimeLimit.ZERO_MINUTE_H:
                str = "0分30秒";
                break;
            case DataBase.TimeLimit.ONE_MINUTE:
                str = "1分00秒";
                break;
            case DataBase.TimeLimit.ONE_MINUTE_H:
                str = "1分30秒";
                break;
            case DataBase.TimeLimit.TWO_MINUTE:
                str = "2分00秒";
                break;
            case DataBase.TimeLimit.TWO_MINUTE_H:
                str = "2分30秒";
                break;
            case DataBase.TimeLimit.TREE_MINUTE:
                str = "3分00秒";
                break;
            default:
                str = "不正な値が参照されました。";
                Debug.LogError("Invalid value");
                break;
        }

        //テキストに代入
        timeLimitValueText.text = str;

    }

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
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        entryTexts[index].entryText.gameObject.SetActive(flags);
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
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        entryTexts[index].entryText.text = entryText;
    }

    /// <summary>
    /// キャラクター選択のテキストをセット
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectCharacterText(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        entryTexts[index].entryText.text = selectCharaText;
    }

    /// <summary>
    /// 準備完了時のテキスト
    /// </summary>
    /// <param name="index"></param>
    public void SetReadyText(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        entryTexts[index].entryText.text = readyText;
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
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
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
        var entryBackRef = entryTexts[index].textBackGround;

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
    /// モデルの番号に応じたキャラ名をセットする
    /// </summary>
    /// <param name="modelNo"></param>
    public void SetCharacterName(int index,int modelNo)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        string name;
        //名前を割り当てる
        switch (modelNo)
        {
            //こはくちゃん
            case (int)DataBase.MODEL.UTC_S:
                name = SKohakuName;
                break;
            case (int)DataBase.MODEL.UTC_W:
                name = WKohakuName;
                break;

            //みさきちゃん
            case (int)DataBase.MODEL.MISAKI_S:
                name = SMisakiName;
                break;
            case (int)DataBase.MODEL.MISAKI_W:
                name = WMisakiName;
                break;

            //ゆうこちゃん
            case (int)DataBase.MODEL.YUKO_S:
                name = SYukoName;
                break;
            case (int)DataBase.MODEL.YUKO_W:
                name = WYukoName;
                break;

            //その他
            default:
                name = noEntryNameText;
                break;
        }

        entryTexts[index].nameText.text = name;
    }

    /// <summary>
    /// キャラクター選択の矢印のアクティブ一括(左右)変更
    /// </summary>
    /// <param name="index"></param>
    /// <param name="flags"></param>
    public void SetCharacterArrowActive(int index,bool flags)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        entryTexts[index].characterLeftArrowImage.gameObject.SetActive(flags);
        entryTexts[index].characterRightArrowImage.gameObject.SetActive(flags);
    }

    /// <summary>
    /// キャラクター選択の矢印の移動開始命令
    /// </summary>
    /// <param name="index"></param>
    public void StartArrowCursol(int index)
    {
        //下限
        if (index < 0)
        {
            Debug.LogError("<color=red>" + "index is under limit over" + "</color>");
            return;
        }
        //上限
        if (index > entryTexts.Length - 1)//インスタンス生成数－1
        {
            Debug.LogError("<color=red>" + "index is top limit over" + "</color>");
            return;
        }

        //既に動いていた場合は停止してあげる
        if (entryTexts[index].coroutine != null)
        {
            StopCoroutine(entryTexts[index].coroutine);
        }
        entryTexts[index].coroutine = MoveArrow(index);
        StartCoroutine(entryTexts[index].coroutine);
    }

    /// <summary>
    /// キャラクター選択の矢印の移動処理
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    IEnumerator MoveArrow(int index)
    {
        var data = entryTexts[index];
        Vector3 leftDir = Vector2.left;
        Vector3 rightDir = Vector2.right;

        while (true)
        {

            //外側
            for(int i = 0; i < arrowMoveFrame; ++i)
            {
                data.characterLeftArrowImage.rectTransform.localPosition += leftDir * arrowMovePower;
                data.characterRightArrowImage.rectTransform.localPosition += rightDir * arrowMovePower;
                yield return null;
            }

            //内側
            for (int i = 0; i < arrowMoveFrame; ++i)
            {
                data.characterLeftArrowImage.rectTransform.localPosition += rightDir * arrowMovePower;
                data.characterRightArrowImage.rectTransform.localPosition += leftDir * arrowMovePower;
                yield return null;
            }

            //補正
            data.characterLeftArrowImage.rectTransform.localPosition = data.initLeftArrowPos;
            data.characterRightArrowImage.rectTransform.localPosition = data.initRightArrowPos;

            yield return null;
        }
    }

    /// <summary>
    /// UIを出現させるためのコルーチンの開始命令
    /// </summary>
    public void StartArrivalReadyGoUI()
    {
        arrivalReadyGoCoroutine = ArrivalReadyGoUI();
        StartCoroutine(arrivalReadyGoCoroutine);
    }

    /// <summary>
    /// UIを非表示にするためのメソッド(座標を初期位置に戻しているだけ)
    /// </summary>
    public void ResetReadyGoUI()
    {
        if (arrivalReadyGoCoroutine != null)
        {
            StopCoroutine(arrivalReadyGoCoroutine);
        }
        readyGoBackFrame.rectTransform.localPosition = initBackFramePos;
        readyGoTextObject.transform.localPosition = initTextPos;
    }

    /// <summary>
    /// 準備完了のUI出現
    /// </summary>
    /// <returns></returns>
    IEnumerator ArrivalReadyGoUI()
    {
        //目標座標が Vector3( 0, 0, 0 )のため (移動先 - 移動元) で距離を算出して1フレーム辺りの移動量を計算する
        float backMoveValue = Mathf.Abs(readyGoBackFrame.rectTransform.localPosition.x) / readyGoMoveFrame;
        float readyGoMoveValue= Mathf.Abs(readyGoTextObject.transform.localPosition.x) / readyGoMoveFrame;

        for (int i = 0; i < readyGoMoveFrame; ++i)
        {
            //背景のフレームは左から右へ移動
            var rectPos = readyGoBackFrame.rectTransform.localPosition;
            rectPos.x += backMoveValue;
            readyGoBackFrame.rectTransform.localPosition = rectPos;

            //TMPのテキストは右から左へ移動
            var pos = readyGoTextObject.transform.localPosition;
            pos.x -= readyGoMoveValue;
            readyGoTextObject.transform.localPosition = pos;

            yield return null;
        }

        //参照を外す
        arrivalReadyGoCoroutine = null;

        yield break;
    }

    /// <summary>
    /// 準備完了のテキストとその背景の現在位置を初期位置としてセット
    /// </summary>
    [ContextMenu("Set ReadyGo init position!")]
    private void SetReadyGoTextAndBackFrameInitializePosition()
    {
        initBackFramePos = readyGoBackFrame.rectTransform.localPosition;
        initTextPos = readyGoTextObject.transform.localPosition;
    }

    /// <summary>
    /// キャラクター選択時の矢印の現在位置を初期位置としてセット
    /// </summary>
    [ContextMenu("Set Arrow init position!")]
    private void SetArrowInitializePosition()
    {
        int i = 0;

        //繰り返し
        while (i < entryTexts.Length)
        {
            var tmp = entryTexts[i];
            tmp.initLeftArrowPos = tmp.characterLeftArrowImage.rectTransform.localPosition;
            tmp.initRightArrowPos = tmp.characterRightArrowImage.rectTransform.localPosition;
            entryTexts[i] = tmp;
            ++i;
        }
    }
}

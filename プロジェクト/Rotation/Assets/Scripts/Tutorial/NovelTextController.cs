using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection;

/// <summary>
/// ノベルゲーム的なテキストの管理
/// </summary>
public class NovelTextController : SingletonMonoBehaviour<NovelTextController> {

    //  serialize param!
    [SerializeField] Sprite[] utcSprite2D;
    [SerializeField] UnityEngine.UI.Image utcImage;
    [SerializeField] NovelTextData[] startMessages;  //表示データ
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] float intervalForCharacterDisplay = 0.05f;//1文字の表示にかかる時間


    //  private param!
    private int currentLine = 0;                //表示テキストのインデックス
    private string currentText = string.Empty;  //現在の文字列
    private float timeUntilDisplay;             //表示にかかる時間
    private float timeElapsed;                  //文字列の表示を開始した時間                 
    private int lastUpdateTextLength;           //表示中の文字数

    //  property
    //  文字の表示が完了しているかどうか
    bool isCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

	// Use this for initialization
	void Start () {

        timeUntilDisplay = 0;
        timeElapsed = 1;
        currentLine = 0;
        lastUpdateTextLength = -1;
        SetNextLine();
	}
	
	// Update is called once per frame
	void Update () {
        TextUpdate();
	}

    public void TextUpdate()
    {
        //  全コントローラーの入力を受け付ける
        var input = MyInputManager.AllController;

        //  テキストが表示しきっている
        if (isCompleteDisplayText)
        {
            //  最後のテキストを表示していたら処理しない
            if (currentLine > startMessages.Length) { return; }

            //  Aボタンを押すことで次のテキストの表示を開始する
            if (input.A)
            {
                SetNextLine();
            }
        }
        //  テキストの表示が終わっていない
        else
        {
            //  Aボタンを押すことですべて表示する
            if (input.A) { timeUntilDisplay = 0; }
        }


        //  テキストの更新処理

        //  現在の時間に表示するべき文字数
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

        //  表示すべき時間と最後に更新された文字数が異なればテキストの文字を更新
        if (displayCharacterCount != lastUpdateTextLength)
        {
            uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateTextLength = displayCharacterCount;
        }
    }

    void SetNextLine()
    {
        //スプライトを変更する
        utcImage.sprite = utcSprite2D[startMessages[currentLine].imageIndex];

        currentText = startMessages[currentLine].text;
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;
        currentLine++;
        lastUpdateTextLength = -1;
    }

    /// <summary>
    /// データの中身のイベント関数の実行
    /// ※関数名は本スクリプト内のみのもの限定
    /// </summary>
    //public void RunEvent()
    //{
    //    //メソッドデータの作成
    //    MethodInfo info = Instance.GetType().GetMethod(data[index].eventFuncName);

    //    //読み込めれば実行
    //    if (info != null) info.Invoke(Instance, null);

    //}
}

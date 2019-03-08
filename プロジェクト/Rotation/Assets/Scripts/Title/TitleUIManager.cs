using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// タイトルUIを管理
/// </summary>
public class TitleUIManager : SingletonMonoBehaviour<TitleUIManager> {

    [SerializeField] GameObject menuObject;
    [SerializeField] TextMeshProUGUI startTextUI;

    [Header("Text Control Parameter")]
    [SerializeField, Tooltip("フロートイン/アウトするフレーム")] float floatFrame = 25;
    [SerializeField] float scaleFrame = 10;
    [SerializeField] float scaleMag = 2.0f;

    [System.Serializable]
    struct MenuTexts
    {
        //public Image textBackGround;
        public TextMeshProUGUI text;
        public Vector3 initScale;
        public IEnumerator coroutine;
        [System.NonSerialized] public bool isRunning;
    }

    [Header("Menu Control Parameter")]
    [SerializeField] MenuTexts playMenu;
    [SerializeField] MenuTexts creditMenu;
    [SerializeField] MenuTexts endMenu;
    //[Header("Color Parameter")]
    //[SerializeField] Color activeColor;
    //[SerializeField] Color disableColor;

    /// <summary>
    /// リセット時の処理
    /// </summary>
    private void Reset()
    {
        //ColorUtility.TryParseHtmlString("#FF9800", out activeColor);
        //ColorUtility.TryParseHtmlString("#D1D1D1", out disableColor);
    }

    /// <summary>
    /// メニューを出現させる
    /// </summary>
    public void PopUpMenu()
    {
        //テキストをDisable
        startTextUI.gameObject.SetActive(false);

        //メニューをActivate
        menuObject.SetActive(true);
    }

    /// <summary>
    /// テキストのフロート開始命令
    /// </summary>
    public void StartFloatText()
    {
        StartCoroutine(StartTextFloat());
    }

    /// <summary>
    /// メニューの設定
    /// </summary>
    /// <param name="menu"></param>
    public void SetMenu(TitleScene.Menu menu)
    {
        switch (menu)
        {
            //あそぶ
            case TitleScene.Menu.Play:
                ActiveMenu(ref playMenu);
                DisableMenu(ref creditMenu);
                DisableMenu(ref endMenu);
                break;
            //くれじっと
            case TitleScene.Menu.Credit:
                DisableMenu(ref playMenu);
                ActiveMenu(ref creditMenu);
                DisableMenu(ref endMenu);
                break;
            //おわる
            case TitleScene.Menu.End:
                DisableMenu(ref playMenu);
                DisableMenu(ref creditMenu);
                ActiveMenu(ref endMenu);
                break;
        }

    }

    /// <summary>
    /// "スタートボタンを押してください"のフロート処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTextFloat()
    {
        //元の色を記憶しておく
        Color origin = startTextUI.color;

        //1フレームあたりに変化させる割合
        float power = origin.a / floatFrame;

        while (true)
        {
            Color cr = startTextUI.color;

            //フロートアウト
            while (cr.a > 0)
            {
                cr.a -= power;
                startTextUI.color = cr;

                yield return null;
            }

            //補正してあげる
            cr.a = 0;

            //フロートイン
            while (cr.a < origin.a)
            {
                cr.a += power;
                startTextUI.color = cr;

                yield return null;
            }

            //補正してあげる
            cr.a = origin.a;

            yield return null;
        }
    }

    /// <summary>
    /// メニューのアクティブ化
    /// </summary>
    /// <param name="menu"></param>
    void ActiveMenu(ref MenuTexts menu)
    {
        //既に動いていたら処理はしない
        if (menu.coroutine!=null) { return; }

        //コルーチンの登録処理
        menu.coroutine = ScalingCoroutine(menu);

        //実行
        StartCoroutine(menu.coroutine);
    }

    /// <summary>
    /// メニューの非アクティブ化
    /// </summary>
    /// <param name="menu"></param>
    void DisableMenu(ref MenuTexts menu)
    {
        //動いていたものを破棄
        if (menu.coroutine != null)
        {
            StopCoroutine(menu.coroutine);
            menu.text.transform.localScale = menu.initScale;//大きさを元に戻してあげる
            menu.isRunning = false;                         //フラグリセット
            menu.coroutine = null;                          //初期化
        }
    }

    /// <summary>
    /// スケーリングを行うコルーチン
    /// </summary>
    /// <param name="menu"></param>
    /// <returns></returns>
    IEnumerator ScalingCoroutine(MenuTexts menu)
    {
        menu.isRunning = true;
        float value = (scaleMag - 1) / scaleFrame;
        while (true)
        {
            //拡大
            for (int i = 0;i < scaleFrame; ++i){
                var tmp = menu.text.gameObject.transform.localScale;
                tmp.x += value;
                tmp.y += value;
                tmp.z += value;

                menu.text.gameObject.transform.localScale = tmp;

                yield return null;
            }
            yield return null;

            //縮小
            for (int i = 0; i < scaleFrame; ++i)
            {
                var tmp = menu.text.gameObject.transform.localScale;
                tmp.x -= value;
                tmp.y -= value;
                tmp.z -= value;

                menu.text.gameObject.transform.localScale = tmp;
                yield return null;
            }
            yield return null;
        }
    }

}

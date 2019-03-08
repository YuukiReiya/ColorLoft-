using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



/// <summary>
/// リザルトシーンのメニューを管理
/// </summary>
public class ResultMenuManager : SingletonMonoBehaviour<ResultMenuManager> {

    [SerializeField] Image backFade;        //メニュー表示時に表示する背景

    [System.Serializable]
    struct MenuTexts
    {
        public Image textBackGround;
        public TextMeshProUGUI text;
    }
    [SerializeField] MenuTexts retryMenu;
    [SerializeField] MenuTexts entryMenu;
    [SerializeField] MenuTexts titleMenu;
    [SerializeField] Color activeColor;
    [SerializeField] Color disableColor;

    /// <summary>
    /// リセット時の処理
    /// </summary>
    private void Reset()
    {
        ColorUtility.TryParseHtmlString("#FF9800", out activeColor);
        ColorUtility.TryParseHtmlString("#D1D1D1", out disableColor);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        //背景のアクティブ化
        backFade.gameObject.SetActive(true);

        //メニューのアクティブ化
        SetActivateMenuTextsObject(retryMenu);
        SetActivateMenuTextsObject(entryMenu);
        SetActivateMenuTextsObject(titleMenu);

        //メニュー初期化(初期は"もういちど"にカーソルを合わせる)
        SetMenu(ResultScene.Menu.Retry);
    }

    /// <summary>
    /// 選択メニューの切り替え
    /// </summary>
    /// <param name="menu"></param>
    public void SetMenu(ResultScene.Menu menu)
    {
        switch (menu)
        {
            //もういちど
            case ResultScene.Menu.Retry:
                ActiveMenu(retryMenu);
                DisableMenu(entryMenu);
                DisableMenu(titleMenu);
                break;
            //キャラクター選択
            case ResultScene.Menu.Entry: 
                DisableMenu(retryMenu);
                ActiveMenu(entryMenu);
                DisableMenu(titleMenu);
                break;
            //たいとる
            case ResultScene.Menu.Title: 
                DisableMenu(retryMenu);
                DisableMenu(entryMenu);
                ActiveMenu(titleMenu);
                break;
        }
    }

    /// <summary>
    /// メニューのオブジェクトのアクティブ化
    /// </summary>
    void SetActivateMenuTextsObject(MenuTexts menu)
    {
        menu.textBackGround.gameObject.SetActive(true);
        menu.text.gameObject.SetActive(true);
    }

    /// <summary>
    /// メニューのアクティブ化
    /// </summary>
    /// <param name="menu"></param>
    void ActiveMenu(MenuTexts menu)
    {
        menu.textBackGround.color = activeColor;
    }

    /// <summary>
    /// メニューの非アクティブ化
    /// </summary>
    /// <param name="menu"></param>
    void DisableMenu(MenuTexts menu)
    {
        menu.textBackGround.color = disableColor;
    }
}

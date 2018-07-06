/// <summary>
/// 番場宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// フェード管理クラス
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class FadeManager : SingletonMonoBehaviour<FadeManager> {

    //Hide variable
    private CanvasGroup canvasGroupEntity;

    //accessor
    public CanvasGroup canvasGroup
    {
        get
        {
            //null check!
            if (canvasGroupEntity == null)
            {
                //GetComponent
                canvasGroupEntity = GetComponent<CanvasGroup>();
                //null check!
                if (canvasGroupEntity == null)
                {
                    //AddComponent
                    canvasGroupEntity = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroupEntity;
        }
    }

    public float Alpha
    {
        get
        {
            return canvasGroup.alpha;
        }
        set
        {
            canvasGroup.alpha = value;
        }
    }

    public bool IsFade
    {
        get
        {
            return state != FadeState.None;
        }
    }

    //enum
    private enum FadeState { None, FadeIn, FadeOut }
    private FadeState state;

    //CallBack
    public delegate void FadeInFinishedFunc();
    public delegate void FadeOutFinishedFunc();

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        GameObject obj = Instance.gameObject;
        
        //add component!
        obj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        obj.AddComponent<GraphicRaycaster>();
        Canvas canvas = Instance.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        //create child!
        GameObject image = new GameObject("FadeImage");
        image.transform.SetParent(Instance.transform, false);
        image.AddComponent<Image>().color = Color.black;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(2000, 2000);

        //  param initialize! 
        state = FadeState.None;
        Alpha = 0;
    }

    /// <summary>
    /// フェードインするコルーチン
    /// </summary>
    /// <param name="frame">フェードさせるフレーム</param>
    /// <param name="fadeInFinished">フェードイン終了後に行う関数</param>
    /// <returns></returns>
    public IEnumerator FadeInCoroutine(float frame, FadeInFinishedFunc fadeInFinished = null)
    {
        state = FadeState.FadeIn;
        float speed = 1 / frame;

        for (int i = 0; i < frame; i++)
        {
            Alpha += speed;
            yield return null;
        }

        if (fadeInFinished != null)
        {
            fadeInFinished();
        }
        state = FadeState.None;
        yield break;
    }

    /// <summary>
    /// フェードアウトするコルーチン
    /// </summary>
    /// <param name="frame">フェードさせるフレーム</param>
    /// <param name="fadeInFinished">フェードアウト終了後に行う関数</param>
    /// <returns></returns>
    public IEnumerator FadeOutCoroutine(float frame,FadeOutFinishedFunc fadeOutFinished = null)
    {
        state = FadeState.FadeOut;
        float speed = 1 / frame;

        for (int i = 0; i < frame; i++)
        {
            Alpha -= speed;
            yield return null;
        }

        if (fadeOutFinished != null)
        {
            fadeOutFinished();
        }
        state = FadeState.None;
        yield break;
    }

    /// <summary>
    /// シーンのフェード
    /// </summary>
    /// <param name="frame">シーンさせるフレーム</param>
    /// <param name="fadeInFinished">フェードイン後に行う関数</param>
    /// <returns></returns>
    public IEnumerator SceneFadeCoroutine(float frame,FadeInFinishedFunc fadeInFinished)
    {
        state = FadeState.FadeIn;
        float speed = 1 / frame;

        for (int i = 0; i < frame; i++)
        {
            Alpha += speed;
            yield return null;
        }

        //Finished Func
        fadeInFinished();

        //update state
        state = FadeState.FadeOut;

        //out
        for (int i = 0; i < frame; i++)
        {
            Alpha -= speed;
            yield return null;
        }

        //end state
        state = FadeState.None;

        yield break;
    }
}

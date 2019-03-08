using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FPSのデバッグ表示
/// </summary>
public class DisplayFPS : SingletonMonoBehaviour<DisplayFPS> {

    //private param!
    [SerializeField, Tooltip("計測制度 指定数秒毎にフレームの更新を行う")] float accuracy = 0.5f;
    [SerializeField, Tooltip("表示する数字のイメージ")] Sprite[] no;
    [Header("DisplayImages")]
    [SerializeField, Tooltip("表示するUI10の位")] Image tenImage;
    [SerializeField, Tooltip("表示するUI1の位")] Image oneImage;
    [SerializeField, Tooltip("表示するUI少数第一位")] Image fewOneImage;

    int frameCount;
    float prevTime;
    float time;
    public float FPS  { get{ return frameCount / time; } }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Start()
    {
        frameCount = 0;
        prevTime = 0.0f;
        time = 1.0f;
    }

    /// <summary>
    /// FPSの更新処理
    /// </summary>
    public void Update()
    {
        //カウンタ加算
        ++frameCount;

        //経過時間の差分取得
        time = Time.realtimeSinceStartup - prevTime;

        //経過時間が指定数秒を超えたら更新
        if (time >= accuracy)
        {
            float fps = frameCount / time;

            int ten = (int)fps / 10 % 10;
            int one = (int)fps % 10;
            int few = (int)(fps * 10 % 10);

            tenImage.sprite = no[ten];
            oneImage.sprite = no[one];
            fewOneImage.sprite = no[few];

            //カウンタのリセット
            frameCount = 0;

            //差分を算出するために経過時間を記憶
            prevTime = Time.realtimeSinceStartup;
        }
    }
}

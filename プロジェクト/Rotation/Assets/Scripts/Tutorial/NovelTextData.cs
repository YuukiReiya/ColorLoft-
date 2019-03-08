using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テキストデータ
/// </summary>
[System.Serializable]
public class NovelTextData  {

    public delegate void Event();
    [TextArea]public string text;     //テキストデータ
    public int imageIndex;  //表示する画像のインデックス
    public string eventFuncName;

}

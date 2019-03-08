using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 空のオブジェクトを生成するだけのコンテキストメニュー
/// </summary>
public class GameObjectFactor : MonoBehaviour {

    [ContextMenu("CreateEmptyGameObject")]
    public void CreateEmpty()
    {
        GameObject Empty = new GameObject("Empty");
    }
}

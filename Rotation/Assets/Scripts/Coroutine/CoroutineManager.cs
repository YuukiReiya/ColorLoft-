using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviourを継承していないスクリプトから
/// コルーチンを呼び出せるようにするコルーチンの管理クラス
/// </summary>
public class CoroutineManager : SingletonMonoBehaviour<CoroutineManager> {

    /// <summary>
    /// コルーチンの代理開始メソッド
    /// </summary>
    /// <param name="coroutine"></param>
    public void StartCoroutineMethod(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

}

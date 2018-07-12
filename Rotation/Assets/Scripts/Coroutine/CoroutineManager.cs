using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

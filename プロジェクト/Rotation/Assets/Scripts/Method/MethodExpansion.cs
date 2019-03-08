//参考：http://kan-kikuchi.hatenablog.com/entry/DelayMethod
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拡張メソッドの名前空間
/// </summary>
namespace MethodExpansion
{
    public static class MethodExpansion
    {
        public delegate void Function();

        /// <summary>
        /// 指定フレーム遅延後に関数の実行を行う
        /// </summary>
        public static void DelayMethod(this MonoBehaviour self,Function func,float frame)
        {
            CoroutineExpansion.CoroutineFinishedFunc cast = ()=> { func(); } ;
            CoroutineExpansion.StartCoroutine(self, WaitCoroutine(self, frame), cast);
        }

        /// <summary>
        /// 指定フレーム何もしないコルーチン
        /// </summary>
        /// <param name="self"></param>
        /// <param name="waitFrame"></param>
        /// <returns></returns>
        public static IEnumerator WaitCoroutine(this MonoBehaviour self, float waitFrame)
        {
            for (int i = 0; i < waitFrame; ++i) { yield return null; }
        }
    }
    /// <summary>
    /// コルーチンの拡張
    /// </summary>
    public static class CoroutineExpansion
    {

        public delegate void CoroutineFinishedFunc();

        /// <summary>
        /// StartCoroutineのオーバーロード
        /// コルーチンの終了後に関数を実行
        /// </summary>
        /// <param name="self"></param>
        /// <param name="routine"></param>
        /// <param name="func"></param>
        public static void StartCoroutine(this MonoBehaviour self,IEnumerator routine, CoroutineFinishedFunc func)
        {
            self.StartCoroutine(DelayRoutine(self, routine, func));
        }

        /// <summary>
        /// 指定フレーム遅延後、コルーチンの呼び出しを行う関数
        /// </summary>
        /// <param name="self"></param>
        /// <param name="routine"></param>
        /// <param name="frame"></param>
        public static void StartDelayCoroutine(this MonoBehaviour self,IEnumerator routine,float frame)
        {
            self.StartCoroutine(DelayRoutine(self, routine, frame));
        }

        /// <summary>
        /// 指定フレーム遅延後コルーチンの実行をするためのコルーチン
        /// </summary>
        /// <param name="self"></param>
        /// <param name="routine"></param>
        /// <param name="frame"></param>
        private static IEnumerator DelayRoutine(this MonoBehaviour self,IEnumerator routine,float frame)
        {
            for(int i = 0; i < frame; ++i) { yield return null; }
            self.StartCoroutine(routine);
        }

        /// <summary>
        /// コルーチンの終了後に関数を実行するためのコルーチン
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="routine"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static IEnumerator DelayRoutine(MonoBehaviour mono,IEnumerator routine, CoroutineFinishedFunc func)
        {
            yield return mono.StartCoroutine(routine);
            if (func != null) { func(); }
        }
    }

    /// <summary>
    /// Time機能の拡張
    /// ※MonoBehaviorクラスの拡張
    /// </summary>
    public static class TimeExpansion
    {
        static float startTime = 0;
        public static float elapsedTime { get { return Time.time - startTime; } }

        /// <summary>
        /// SetStartTime()で設定した時間からの経過時間を取得
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static float GetElapsedTime(this MonoBehaviour self)
        {
            return elapsedTime;
        }

        /// <summary>
        /// 経過時間を求めるための開始時間を呼び出した時間に設定
        /// </summary>
        public static void SetStartTime(this MonoBehaviour self)
        {
            startTime = Time.time;
        }

        /// <summary>
        /// 経過時間を求めるための開始時間を0に設定
        /// </summary>
        public static void ResetStartTime(this MonoBehaviour self)
        {
            startTime = 0;
        }
    }
}

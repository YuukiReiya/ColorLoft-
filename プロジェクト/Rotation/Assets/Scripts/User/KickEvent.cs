using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// キックのAnimationEventの設定
/// </summary>
public class KickEvent : MonoBehaviour {


    //  private param!
    Animator animator;

    //  delegate func!
    System.Action kickStartFunc;
    System.Action kickEndFunc;
    System.Action kickVoiceFunc;

    //  const param!
    const int KICK_CLIP_INDEX = 5;
    const float VOICE_CALL_TIME = 0.0f; //キック時のボイス呼び出し用の関数の呼び出し時間(0 ～ 1)
    const float START_CALL_TIME = 0.3f; //キック開始の関数の呼びだし時間(0 ～ 1)
    const float END_CALL_TIME = 0.65f;  //キック終了の関数の呼びだし時間(0 ～ 1)

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init(Animator anim)
    {
        animator = anim;

        //AnimationEventの登録内容
        AnimationEvent startEvt, endEvt,voiceEvt;

        //  キックのトリガーを入れる
        startEvt = new AnimationEvent();
        startEvt.time = START_CALL_TIME;    //呼び出し時間
        startEvt.functionName = "StartFunc";//呼び出し関数

        //  キックのトリガーを切る
        endEvt = new AnimationEvent();
        endEvt.time = END_CALL_TIME;        //呼び出し時間
        endEvt.functionName = "EndFunc";    //呼び出し関数

        //  ボイスを流す
        voiceEvt = new AnimationEvent();
        voiceEvt.time = VOICE_CALL_TIME;    //呼び出し時間
        voiceEvt.functionName = "VoiceFunc";//呼び出し関数

        //AnimationEventをクリップに追加
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[KICK_CLIP_INDEX];

        //var v = new AnimationEvent[100];
        //float f = 0;
        //for(int i=0;i<v.Length;i++)
        //{

        //    v[i] = new AnimationEvent();
        //    v[i].time = f;    //呼び出し時間
        //    v[i].functionName = "TmpFunc";//呼び出し関数
        //    v[i].floatParameter = f;
        //    clip.AddEvent(v[i]);
        //    f += 0.01f;
        //}
        clip.AddEvent(startEvt);
        clip.AddEvent(endEvt);
        clip.AddEvent(voiceEvt);

        //予め生成して起き内容を変更する
        AnimationClip[] clips= animator.runtimeAnimatorController.animationClips;

        foreach (var it in clips)
        {
            //Debug.Log("model = " + animator.gameObject.transform.parent.gameObject.name + "/Eventカウント" + it.events.Length);
            foreach(var t in it.events)
            {
                //Debug.Log("model = " + animator.gameObject.transform.parent.gameObject.name + "クリップName ＝ " + it.name + "/関数名 ≒ " + t.functionName);
            }
        }
      //  Debug.Log("model = " + animator.transform.parent.gameObject.name + "Event個数" + clips.Sum(it => it.events.Length));
    }

    /// <summary>
    /// キックアニメーション時のトリガーを入れる関数の内容
    /// </summary>
    /// <param name="action"></param>
    public void SetKickStartFunc(System.Action action)
    {
        kickStartFunc = action;
    }

    /// <summary>
    /// キックアニメーション時のトリガーを切る関数の内容
    /// </summary>
    /// <param name="action"></param>
    public void SetKickEndFunc(System.Action action)
    {
        kickEndFunc = action;
    }

    /// <summary>
    /// キックのボイス再生用関数のセッター
    /// </summary>
    /// <param name="action"></param>
    public void SetKickVoiceFunc(System.Action action)
    {
        kickVoiceFunc = action;
    }

    /// <summary>
    /// トリガーを入れるAnimationEventを呼び出すための関数
    /// （Playerスクリプトの関数を呼び出すためのラッパーである）
    /// ※AnimationEvent用のスクリプトはAnimatorと同じオブジェクトに付ける必要がある
    /// </summary>
    private void StartFunc()
    {
        //メソッドが登録されていたら実行する
        if (kickStartFunc != null)
        {
            kickStartFunc();
        }
    }
    /// <summary>
    /// トリガーを切るAnimationEventを呼び出すための関数
    /// （Playerスクリプトの関数を呼び出すためのラッパーである）
    /// ※AnimationEvent用のスクリプトはAnimatorと同じオブジェクトに付ける必要がある
    /// </summary>
    private void EndFunc()
    {
        //メソッドが登録されていたら実行する
        if (kickEndFunc != null)
        {
            kickEndFunc();
        }
    }

    /// <summary>
    /// キック時のボイス再生用のAnimationEventを呼び出すための関数
    /// （Playerスクリプトの関数を呼び出すためのラッパーである）
    /// ※AnimationEvent用のスクリプトはAnimatorと同じオブジェクトに付ける必要がある
    /// </summary>
    private void VoiceFunc()
    {
        if (kickVoiceFunc != null)
        {
            kickVoiceFunc();
        }
    }
}

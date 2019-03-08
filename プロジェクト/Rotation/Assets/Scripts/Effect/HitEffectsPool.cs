using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HitEffectsPool : SingletonObjectPoolBase<HitEffectsPool> {

    /// <summary>
    /// ポーズ処理
    /// </summary>
    public bool isPause { get; set; }

    public void Run(Vector3 pos)
    {
        StartCoroutine(Work(pos));
    }

    IEnumerator Work(Vector3 pos)
    {
        var inst = GetObject();
        inst.transform.position = pos;

        //この時点でNullの場合、poolObjectにParticleSystemがアタッチされていません
        var effects = inst.GetComponentsInChildren<ParticleSystem>();

        //エフェクト再生
        foreach (var it in effects)
        {
            it.Play();
        }

        //エフェクト終了まで待機
        while (effects.Any(fx => fx.IsAlive()))
        {
            update(effects);
            yield return null;
        }

        //プールに戻す処理
        inst.SetActive(false);
    }

    /// <summary>
    /// ポーズ
    /// </summary>
    /// <param name="effects"></param>
    void Pause(ParticleSystem[] effects)
    {
        foreach(var it in effects) { it.Pause(); }
    }

    /// <summary>
    /// ポーズ解除
    /// </summary>
    /// <param name="effects"></param>
    void UnPause(ParticleSystem[] effects)
    {
        foreach (var it in effects) { it.Play(); }
    }

    /// <summary>
    /// 更新
    /// </summary>
    void update(ParticleSystem[] effects)
    {
        bool effectsPause = effects.All(it => it.isPaused);//エフェクトが一時停止しているか

        //一時停止の瞬間
        if (isPause && !effectsPause)
        {
            Pause(effects);
        }
        //一時停止の解除の瞬間
        else if (!isPause && effectsPause)
        {
            UnPause(effects);
        }
    }
}

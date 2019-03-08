using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// IKを利用してターゲットの方に向く
/// </summary>
public class LookAtTargetIK : MonoBehaviour {

    [SerializeField]Animator animator;
    Vector3? targetPos;

    float weight;       //(0 ～ 1)他のパラメータの係数
    float bodyWeight;   //(0 ～ 1)身体のLookAtに関係する割合
    float headWeight;   //(0 ～ 1)頭のLookAtに関係する割合
    float eyeWeight;    //(0 ～ 1)目のLookAtに関係する割合
    float clampWeight;  //(0 ～ 1) 0.0:制限がゼロ、1.0:完全に制限 0.5:使用可能な範囲の半分(180度)

    /// <summary>
    /// リセット時の処理
    /// </summary>
    private void Reset()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// アニメーターのIK関連のコールバック
    /// </summary>
    /// <param name="layerIndex"></param>
    private void OnAnimatorIK(int layerIndex)
    {
        //ターゲットが設定されてなければ処理はしない
        if (targetPos == null) return;

        Vector3 pos = (Vector3)targetPos;

        this.animator.SetLookAtWeight(weight, bodyWeight, headWeight, eyeWeight, clampWeight);


        //注視
        this.animator.SetLookAtPosition(pos);
    }

    /// <summary>
    /// ターゲットの設定
    /// </summary>
    /// <param name="target">nullを指定した場合LookAt処理をキャンセル</param>
    public void SetTargetPosition(Vector3? targetPos)
    {
        this.targetPos = targetPos;
    }

    /// <summary>
    /// アニメーターのSetLookAtWeightのラッピング
    /// </summary>
    /// <param name="weight">(0 ～ 1)他のパラメータの係数</param>
    /// <param name="bodyWeight">(0 ～ 1)身体のLookAtに関係する割合</param>
    /// <param name="headWeight">(0 ～ 1)頭のLookAtに関係する割合</param>
    /// <param name="eyeWeight">(0 ～ 1)目のLookAtに関係する割合</param>
    /// <param name="clampWeight">(0 ～ 1) 0.0:制限がゼロ、1.0:完全に制限 0.5:使用可能な範囲の半分(180度) </param>
    public void SetLookAtWeight(float weight,float bodyWeight,float headWeight,float eyeWeight,float clampWeight)
    {
        this.weight         = weight;
        this.bodyWeight     = bodyWeight;
        this.headWeight     = headWeight;
        this.eyeWeight      = eyeWeight;
        this.clampWeight    = clampWeight;
    }
}

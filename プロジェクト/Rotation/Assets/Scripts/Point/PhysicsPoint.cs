using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理挙動有のポイント(プレイヤーから放出されるやつ)
/// </summary>
public class PhysicsPoint : MonoBehaviour {

    [SerializeField,] LayerMask playerMask = 1 << 9;

    /// <summary>
    /// 衝突判定(トリガー)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //  レイヤーマスク
        LayerMask mask = 1 << other.gameObject.layer;

        //  マスク判定に失敗したら処理しない
        if (playerMask != mask) { return; }

        //  Player取得
        Player player = other.transform.parent.GetComponent<Player>();

        if (!player) { return; }


        //  加点処理
        int point = PointManager.SMALL_POINT;
        player.AddPoint(point);

        //  加点UI表示
        PointUIPools.Instance.DrawPositiveScore(
            player.ContollerIndex,
            this.transform.position,
            point
            );

        //  効果音
        float volume = PointManager.GET_POINT_SE_VOLUME;
        SoundManager.Instance.PlayOnSE("GetPoint", volume);

        //  消滅(プールに戻す)
        gameObject.SetActive(false);
    }

}

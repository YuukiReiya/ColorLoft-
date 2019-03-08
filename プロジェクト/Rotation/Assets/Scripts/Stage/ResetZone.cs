using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ResetZone : MonoBehaviour {

    //  private param!
    [SerializeField,] private LayerMask playerLayer;
    [SerializeField,] private LayerMask gridLayer = 1 << 10;
    [SerializeField,] private LayerMask pointlayer = 1 << 11;

    //  const param!
    const int RevivalCounts = 1;    //復活するかしないかを判定するラインの個数(この個数を未満なら復活しない)

    //  delegate
    delegate void Func();

    /// <summary>
    /// トリガーの衝突
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        LayerMask layer = 1 << other.gameObject.layer;

        //判定

        //ステージ
        if (layer == gridLayer)
        {
            HitGrid(other);
        }

        //プレイヤー
        else if(layer == playerLayer)
        {
            HitPlayer(other);
        }

        //ポイント
        else if (layer == pointlayer)
        {
            HitPoint(other);
        }

    }

    /// <summary>
    /// グリッドのResetZoneの衝突
    /// </summary>
    /// <param name="other"></param>
    private void HitGrid(Collider other)
    {
        //親オブジェクト
        var parent = other.GetComponent<Grid>().gameObject.transform.parent;

        //親オブジェクトに付いているTypeL取得
        TypeL typeL = parent.GetComponent<TypeL>();
        if (typeL)
        {

            //  All Freeze!
            RigidbodyConstraints mask = RigidbodyConstraints.FreezeAll;

            typeL.SetConstraints(mask);
            typeL.ResetPosition();
            typeL.StartRevival();
        }

    }

    /// <summary>
    /// プレイヤーのResetZoneの衝突
    /// </summary>
    /// <param name="other"></param>
    private void HitPlayer(Collider other)
    {
        Player player = other.transform.parent.GetComponent<Player>();
        if (!player) { return; }

        //死亡処理
        Die(player,other.gameObject);

    }

    /// <summary>
    /// ポイントのResetZoneの衝突
    /// </summary>
    /// <param name="other"></param>
    private void HitPoint(Collider other)
    {
        PhysicsPoint point = other.GetComponent<PhysicsPoint>();
        if (!point) { return; }

        //プールに戻す
        point.gameObject.SetActive(false);
    }

    /// <summary>
    /// プレイヤーの死亡処理
    /// </summary>
    private void Die(Player player,GameObject model)
    {
        //制限
        player.StopMoveAnimation(); //移動アニメーションを停止する
        player.Stock--;             //ストックを減らす

        //残機がなくなり完全に死亡
        //if (player.Stock < RevivalCounts)
        //{

        //}
        //else
        {
            //初期位置に戻す
            player.ResetPosition();

            //ダメージ処理のキャンセル
            player.CancelDamage();

            //ポイントを半分
            player.PointHarf();

            //無敵化
            player.StartInvisible();

        }

    }
}

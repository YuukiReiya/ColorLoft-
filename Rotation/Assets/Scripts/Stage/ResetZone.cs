using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ResetZone : MonoBehaviour {

    //private param!
    [SerializeField,] private LayerMask playerLayer;
    [SerializeField,] private LayerMask gridLayer = 1 << 10;
    [SerializeField,] private float resetSceonds = 5;

    //delegate
    delegate void Func();

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// トリガーの衝突
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        LayerMask layer = 1 << other.gameObject.layer;

        //判定
        if (layer == gridLayer)
        {
            HitGrid(other);
        }

        else if(layer == playerLayer)
        {
            HitPlayer(other);
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
            typeL.SetActiveGrid(false);

            //  All Freeze!
            RigidbodyConstraints mask = RigidbodyConstraints.FreezeAll;

            typeL.SetConstraints(mask);
            typeL.ResetPosition();
            typeL.DelayRevival(10);
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

        player.isMove = false;
        other.gameObject.transform.position = player.resetPos;
        StartCoroutine(
            DelayFunc
            (
            3,
            () => { player.isMove = true; }
            )
         );

    }

    private IEnumerator DelayFunc(float second,Func func)
    {
        int time = (int)(second * 60);

        for(int i = 0; i < time; ++i)
        {
            yield return null;
        }

        if (func != null) { func(); }
    }
}

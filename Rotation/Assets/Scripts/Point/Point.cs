using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポイント
/// </summary>
public class Point : MonoBehaviour {

    [SerializeField,] LayerMask playerMask = 1 << 9;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 衝突判定(トリガー)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        LayerMask mask = 1 << other.gameObject.layer;

        if (playerMask != mask) { return; }

        //  Player取得
        Player player = other.transform.parent.GetComponent<Player>();

        if (!player) { return; }

        player.AddPoint();

        PointManager.Instance.CreatePoint();

        //  消滅
        Destroy(this.gameObject);
    }
}

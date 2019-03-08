using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃用のトリガー
/// </summary>
public class AttackTrigger : MonoBehaviour {

    [SerializeField] GameObject player;//自身

    private void Reset()
    {
        player = transform.parent.parent.gameObject;
    }

    //
    private void OnTriggerStay(Collider other)
    {
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 処理
/// </summary>
public class DestroyZone : MonoBehaviour {

 //   [SerializeField, Tooltip("破壊対象マスク")] LayerMask destroyMask;

 //   //private param!
 //   private BoxCollider col;

 //   //public param!


	//// Use this for initialization
	//void Start () {
 //       col = GetComponent<BoxCollider>();
	//}

    private void OnTriggerEnter(Collider other)
    {
        LayerMask mask = 1 << other.gameObject.layer;

        //if (mask == destroyMask)
        //{
        //    GameObject destroyObject = other.gameObject.transform.parent.gameObject;
        //    Destroy(destroyObject);
        //}
    }
}


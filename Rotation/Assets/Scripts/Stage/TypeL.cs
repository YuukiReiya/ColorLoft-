using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// L字型Gridの管理
/// </summary>
public class TypeL : MonoBehaviour {

    //  private param!
    List<GameObject> grids;
    Rigidbody rb;
    Vector3 pos;

    //  call back!
    delegate void Func();

    // Use this for initialization
    void Start () {

        //  create list instance!
        grids = new List<GameObject>();

        foreach (var it in GetComponentsInChildren<Grid>(true))
        {
            grids.Add(it.gameObject);
        }

        //  get Rigidbody!
        rb = GetComponent<Rigidbody>();

        //  reset pos!
        pos = this.transform.position;
	}
	
    /// <summary>
    /// 子オブジェクトのGridのアクティブを変更
    /// </summary>
    /// <param name="active"></param>
    public void SetActiveGrid(bool active)
    {
        foreach(var it in grids)
        {
            it.SetActive(active);
        }
    }

    /// <summary>
    /// RigidBodyのConstraintsを設定
    /// </summary>
    /// <param name="constraints"></param>
    public void SetConstraints(RigidbodyConstraints constraints)
    {
        rb.constraints = constraints;
    }

    /// <summary>
    /// 座標のリセット(初期位置に)
    /// </summary>
    public void ResetPosition()
    {
        this.transform.position = pos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    public void DelayRevival(float time)
    {

        StartCoroutine(
            
            DelayFunc(
                time,
                () => { SetActiveGrid(true); }
                )

            );
    }

    /// <summary>
    /// 遅延後関数実行
    /// </summary>
    /// <param name="second">遅延させる時間(秒)</param>
    /// <param name="func">遅延後に行う関数</param>
    private IEnumerator DelayFunc(float second,Func func)
    {
        int FRAME = (int)(second * SystemManager.Instance.Fps);

        for (int i = 0; i < FRAME; ++i)
        {
            yield return null;
        }

        if (func != null) { func(); }
    }
}

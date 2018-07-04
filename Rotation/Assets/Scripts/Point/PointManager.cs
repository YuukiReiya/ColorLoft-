using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポイントのマネージャークラス
/// </summary>
public class PointManager : SingletonMonoBehaviour<PointManager> {

    //  private param!
    [SerializeField,Tooltip("※pointスクリプトを忘れずにつけておく")] GameObject pointPrefab;

    Vector3[] popPos =
    {
          new Vector3(1.0f,1.0f,12.75f),new Vector3(4.0f,1.0f,12.75f),new Vector3(7.0f,1.0f,12.75f),new Vector3(10.0f,1.0f,12.75f),new Vector3(13.0f,1.0f,12.75f),
          new Vector3(1.0f,1.0f,9.75f),new Vector3(4.0f,1.0f,9.75f),new Vector3(7.0f,1.0f,9.75f),new Vector3(10.0f,1.0f,9.75f),new Vector3(13.0f,1.0f,9.75f),
          new Vector3(1.0f,1.0f,6.75f),new Vector3(4.0f,1.0f,6.75f),new Vector3(7.0f,1.0f,6.75f),new Vector3(10.0f,1.0f,6.75f),new Vector3(13.0f,1.0f,6.75f),
          new Vector3(1.0f,1.0f,3.75f),new Vector3(4.0f,1.0f,3.75f),new Vector3(7.0f,1.0f,3.75f),new Vector3(10.0f,1.0f,3.75f),new Vector3(13.0f,1.0f,3.75f),
          new Vector3(1.0f,1.0f,0.75f),new Vector3(4.0f,1.0f,0.75f),new Vector3(7.0f,1.0f,0.75f),new Vector3(10.0f,1.0f,0.75f),new Vector3(13.0f,1.0f,0.75f),
    };

    // Use this for initialization
    void Start() {

        CreatePoint();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreatePoint()
    {
        int index = 0;
        index = Random.RandomRange(0, popPos.Length);
        Vector3 pos = popPos[index];

        var obj = Instantiate(pointPrefab);

        obj.transform.position = pos;
    }
}

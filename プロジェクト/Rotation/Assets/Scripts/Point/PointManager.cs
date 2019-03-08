using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ポイントのマネージャークラス
/// </summary>
public class PointManager : SingletonMonoBehaviour<PointManager> {

    //  private param!
    [SerializeField,Tooltip("※pointスクリプトを忘れずにつけておく")] GameObject pointPrefab;
    [SerializeField] float randMin = 1.5f;
    [SerializeField] float randMax = 5;


    Vector3[] popPos =
     {
          new Vector3(1.0f,1.0f,12.75f),new Vector3(5.0f,1.0f,12.75f),  new Vector3(8.9f,1.0f,12.75f),  new Vector3(12.85f,1.0f,12.75f),    new Vector3(16.8f,1.0f,12.75f), new Vector3(20.75f,1.0f,12.75f),
          new Vector3(1.0f,1.0f,8.8f),  new Vector3(5.0f,1.0f,8.8f),    new Vector3(8.9f,1.0f,8.8f),    new Vector3(12.85f,1.0f,8.8f),      new Vector3(16.8f,1.0f,8.8f),   new Vector3(20.75f,1.0f,8.8f),
          new Vector3(1.0f,1.0f,5.0f),  new Vector3(5.0f,1.0f,5.0f),    new Vector3(8.9f,1.0f,5.0f),    new Vector3(12.85f,1.0f,5.0f),      new Vector3(16.8f,1.0f,5.0f),   new Vector3(20.75f,1.0f,5.0f),
          new Vector3(1.0f,1.0f,1.0f),  new Vector3(5.0f,1.0f,1.0f),    new Vector3(8.9f,1.0f,1.0f),    new Vector3(12.85f,1.0f,1.0f),      new Vector3(16.8f,1.0f,1.0f),   new Vector3(20.75f,1.0f,1.0f),
    };

    //  const param!
    public const int BIG_POINT = 10;                //ポイント(大)の獲得ポイント
    public const int SMALL_POINT = 1;               //ポイント(小)の獲得ポイント
    public const float GET_POINT_SE_VOLUME = 1.0f;  //ポイント獲得時のSEの音量
    const float RANDOM_DIRECTION_MIN = -3;          //x方向とz方向のランダム方向を求める最小値
    const float RANDOM_DIRECTION_MAX = 3;           //x方向とz方向のランダム方向を求める最大値
    const float RANDOM_HEIGHT_MIN = 3;              //ランダムな初速度の高さを求める最小値
    const float RANDOM_HEIGHT_MAX = 5;              //ランダムな初速度の高さを求める最大値


    // Use this for initialization
    void Start() {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ポイントの生成
    /// </summary>
    public void CreatePoint()
    {
        //インデックス
        int index = 0;

        //乱数取得
        index = Random.RandomRange(0, popPos.Length);

        //生成位置
        Vector3 pos = popPos[index];

        //生成
        var obj = Instantiate(pointPrefab);

        //代入
        obj.transform.position = pos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    public void CreateRandamPhysicsPoint(Vector3 pos)
    {
        //プールからオブジェクトを取得
        var inst = PhysicsPointPool.Instance.GetObject();
        var rigidBody = inst.GetComponent<Rigidbody>();

        //座標を補正
        inst.transform.position = pos;

        //初速度
        float power = Random.Range(randMin, randMax);

        //ベクトル = 方向 * 初速度
        var vec = RandamDirection() * power;

        //力を加える
        rigidBody.AddForce(vec);
    }

    /// <summary>
    /// ランダムな方向(正規化済み)を返す
    /// </summary>
    /// <returns></returns>
    private Vector3 RandamDirection()
    {
        //成分の決定
        float x = Random.Range(RANDOM_DIRECTION_MIN, RANDOM_DIRECTION_MAX);
        float z = Random.Range(RANDOM_DIRECTION_MIN, RANDOM_DIRECTION_MAX);
        float y = Random.Range(RANDOM_HEIGHT_MIN, RANDOM_HEIGHT_MAX);

        //ベクトルに代入
        Vector3 dir = new Vector3(x, y, z);

        //ランダムなベクトルを正規化し、方向ベクトルを返す
        return dir.normalized;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        //PointPopMarkコンポーネントを持ったオブジェクトを取得
        //※(このコンポーネントを持っているオブジェクトの位置に生成するため全部取得しておく)
        //pointsMark = StageManager.Instance.gameObject.GetComponentsInChildren<PointPopMarker>();
    }
}

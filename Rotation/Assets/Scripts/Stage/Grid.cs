#define GIZMOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1マスのデータ
/// </summary>
public class Grid : MonoBehaviour {

    //  grid param!
    public DataBase.COLOR color;

    //  deep copy param!
    //private 


#if GIZMOS
    Color cr;
    Vector3 offset = new Vector3(0, 1, 0);

    private void OnDrawGizmos()
    {
        Gizmos.color = ConvertColor(color);
        Vector3 pos = this.transform.position;
        pos += offset;
        Gizmos.DrawWireCube(pos, this.transform.localScale);
    }

    Color ConvertColor(DataBase.COLOR cr)
    {
        switch (cr)
        {
            case DataBase.COLOR.RED:return Color.red;
            case DataBase.COLOR.BLUE: return Color.blue;
            case DataBase.COLOR.GREEN: return Color.green;
            case DataBase.COLOR.YELLOW: return Color.yellow;
        }
        return Color.white;
    }
#endif

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 指定座標がグリッドの判定区域に侵入しているか判定
    /// </summary>
    /// <param name="pos">判定座標</param>
    /// <returns>true:侵入</returns>
    public bool CheckIntrusion(Vector3 pos)
    {
        //  非アクティブ状態なら判定せずfalseを返す
        if (!this.gameObject.activeSelf)
        {
            return false;
        }

        bool x, z;

        float xHalfScale = this.transform.localScale.x / 2;
        float zHalfScale = this.transform.localScale.z / 2;

        x = (this.transform.position.x - xHalfScale) <= pos.x && pos.x <= (this.transform.position.x + xHalfScale);//横の領域判定
        z = (this.transform.position.z - zHalfScale) <= pos.z && pos.z <= (this.transform.position.z + zHalfScale);//奥の領域判定

        return x && z;
    }
}

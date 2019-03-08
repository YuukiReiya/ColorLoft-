using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 4つのシェアグリッドから構成される1つのブロック
/// </summary>
public class Block : MonoBehaviour {

    //  grid list
    List<Grid> reds;
    List<Grid> blues;
    List<Grid> greens;
    List<Grid> yellows;

    //  typeL list
    List<TypeL> typeLlist;

    private bool allChild = true;//非アクティブな子オブジェクトもGetComponentsInChildrenの対象にするか

    /// <summary>
    /// 一度踏まれているか
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private bool GetIsStepOnOnce(DataBase.COLOR color)
    {
        return isStepOnOnce[(int)color];
    }
    private bool[] isStepOnOnce;

    public bool GetIsFall(DataBase.COLOR color)
    {
        //一度踏まれている状態 && 復活処理をしていなければ

        return GetIsStepOnOnce(color) && !typeLlist[(int)color].GetIsRevival();
    }

    //  Awake Function!
    private void Awake()
    {
        //  create instance!
        reds    = new List<Grid>();
        blues   = new List<Grid>();
        greens  = new List<Grid>();
        yellows = new List<Grid>();
        typeLlist  = new List<TypeL>();
    }

    // Use this for initialization
    void Start () {

        var grids = this.transform.GetComponentsInChildren<Grid>(allChild);

        foreach(var it in grids)
        {
            switch (it.color)
            {
                case DataBase.COLOR.RED:
                    reds.Add(it);
                    continue;
                case DataBase.COLOR.BLUE:
                    blues.Add(it);
                    continue;
                case DataBase.COLOR.GREEN:
                    greens.Add(it);
                    continue;
                case DataBase.COLOR.YELLOW:
                    yellows.Add(it);
                    continue;
            }
        }

        var typeLs = this.transform.GetComponentsInChildren<TypeL>(allChild);

        foreach(var it in typeLs)
        {
            typeLlist.Add(it);
        }

        //flags create!
        isStepOnOnce = new bool[] { false, false, false, false };

    }
	
    /// <summary>
    /// 選択色のグリッドを格納したリストをクリアする
    /// </summary>
    /// <param name="color">色</param>
    public void ClearList(DataBase.COLOR color)
    {
        switch (color)
        {
            case DataBase.COLOR.RED:
                reds.Clear();
                return;
            case DataBase.COLOR.BLUE:
                blues.Clear();
                return;
            case DataBase.COLOR.GREEN: 
                greens.Clear();
                return;
            case DataBase.COLOR.YELLOW:
                yellows.Clear();
                return;
        }
    }

    /// <summary>
    /// isFallフラグのリセット
    /// </summary>
    /// <param name="color"></param>
    public void ResetIsFall(DataBase.COLOR color)
    {
        isStepOnOnce[(int)color] = false;
    }

    /// <summary>
    /// 指定色のグリッドエリアに指定座標が含まれているか(侵入しているか)
    /// </summary>
    /// <param name="pos">判定座標</param>
    /// <param name="color">判定色</param>
    /// <returns>結果</returns>
    public bool CheckColorArea(Vector3 pos,DataBase.COLOR color)
    {
        List<Grid> list = null;

        //  判定するリストが何色か判定
        switch (color)
        {
            case DataBase.COLOR.RED:
                list = reds;
                break;
            case DataBase.COLOR.BLUE:
                list = blues;
                break;
            case DataBase.COLOR.GREEN:
                list = greens;
                break;
            case DataBase.COLOR.YELLOW:
                list = yellows;
                break;
        }

        //  リストのnull判定
        if (list == null)
        {
            return false;
        }

        //  リストの中身の侵入チェック
        foreach(var it in list)
        {
            if (it.CheckIntrusion(pos))
            {
                isStepOnOnce[(int)color] = !typeLlist[(int)color].GetIsRevival();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 指定色のグリッドエリアの落下を開始
    /// ※RigitBodyのFreezePosition(y)を解除
    /// </summary>
    /// <param name="color"></param>
    public void StartFall(DataBase.COLOR color)
    {
        List<Grid> list = null;

        //  判定するリストが何色か判定
        switch (color)
        {
            case DataBase.COLOR.RED:
                list = reds;
                break;
            case DataBase.COLOR.BLUE:
                list = blues;
                break;
            case DataBase.COLOR.GREEN:
                list = greens;
                break;
            case DataBase.COLOR.YELLOW:
                list = yellows;
                break;
        }

        //  リストのnull判定
        if (list == null)
        {
            return;
        }

        //  落下処理
        foreach (var it in list)
        {
            Transform parent = it.transform.parent;
            TypeL typeL= parent.GetComponent<TypeL>();

            if (typeL)
            {
                //Positionのyのみ解除
                RigidbodyConstraints mask =
                    RigidbodyConstraints.FreezeRotation |
                    RigidbodyConstraints.FreezePositionX |
                    RigidbodyConstraints.FreezePositionZ;

                //マスクの割り当て
                typeL.SetConstraints(mask);
                return;
            }
        }
    }
}

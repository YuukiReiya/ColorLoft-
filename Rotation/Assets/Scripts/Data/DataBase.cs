using UnityEngine;


public class DataBase
{
    //IDの算出部
    //3,4桁目:    モデル番号
    //1,2桁目:    色番号
    public static readonly int ID_MODEL = 100;//モデルオフセット
    public static readonly int ID_COLOR  = 1; //色のオフセット

    /// <summary>
    /// IDからモデル部分のみを抜粋
    /// </summary>
    /// <param name="id">元ID</param>
    /// <returns></returns>
    public static int GetModelID(int id)
    {
        return id / ID_MODEL;
    }

    /// <summary>
    /// IDから色部分のみ抜粋
    /// </summary>
    /// <param name="id">元ID</param>
    /// <returns></returns>
    public static int GetColorID(int id)
    {
        int one = id % 10;//1の位
        id /= 10;
        int ten = id % 10;//10の位

        //10の位+1の位
        return ten * 10 + one;
    }

    /// <summary>
    /// 色
    /// </summary>
    public enum COLOR
    {
        RED = 0,
        BLUE,
        GREEN,
        YELLOW
    }

    /// <summary>
    /// モデル
    /// </summary>
    public enum MODEL
    {
        UTC_S = 1,      //Unityちゃん:夏
        UTC_W,          //Unityちゃん:冬
        MISAKI_S,       //みさきちゃん:夏
        MISAKI_W,       //みさきちゃん:冬
        YUKO_S,         //ゆうこちゃん:夏
        YUKO_W,         //ゆうこちゃん:冬
    }
}
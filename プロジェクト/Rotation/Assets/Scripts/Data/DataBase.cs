using UnityEngine;


public class DataBase
{
    //定数部
    public static readonly int PLAYER_NUM = 4;//プレイヤーの最大人数

    //IDの算出部
    //  5桁目:    コントローラー番号
    //3,4桁目:    プレハブ番号
    //1,2桁目:    色番号
    public static readonly int ID_INDEX = 10000; //コントローラーオフセット
    public static readonly int ID_PREFAB = 100;  //プレハブオフセット
    public static readonly int ID_COLOR  = 1;    //色のオフセット

    /// <summary>
    /// IDからコントローラーのインデックス部分のみを抜粋
    /// </summary>
    /// <param name="id">元ID</param>
    /// <returns></returns>
    public static int GetControllerID(int id)
    {
        return id / ID_INDEX;
    }

    /// <summary>
    /// IDからプレハブ部分のみを抜粋
    /// </summary>
    /// <param name="id">元ID</param>
    /// <returns></returns>
    public static int GetPrefabID(int id)
    {
        int tmp= id / ID_PREFAB;

        int one, ten;
        one = tmp % 10;     //1の位
        ten = tmp / 10 % 10;//10の位

        //10の位+1の位
        return ten * 10 + one;
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

    /// <summary>
    /// ゲームモード
    /// </summary>
    public enum MODE
    {
        SCORE_SYSTEM,   //スコア制
        STOCK_SYSTEM    //ストック制
    }

    /// <summary>
    /// ストック数
    /// </summary>
    public enum STOCK
    {
        ONE=1,
        TWO,
        THREE,
        FOUR,
        FIVE,
        INFINITE = 999999,   //∞
    }

    /// <summary>
    /// 制限時間のリスト
    /// </summary>
    public enum TimeLimit
    {
       // INFINITE = -1,   //∞
        ZERO_MINUTE_H = 30,
        ONE_MINUTE = 60,   //1分
        ONE_MINUTE_H = 90,   //1分30秒
        TWO_MINUTE = 120,  //2分
        TWO_MINUTE_H = 150,  //2分30秒
        TREE_MINUTE = 180,  //3分
    }

    /// <summary>
    /// 列挙体内の次の要素を返す
    /// ※末尾を渡した場合先頭が返る
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static TimeLimit GetTimeLimitNext(TimeLimit time)
    {
        //∞
        //if (time == TimeLimit.INFINITE) { return TimeLimit.ONE_MINUTE; }
        //Tail
        /*else*/ if (time == TimeLimit.TREE_MINUTE) { return TimeLimit.ZERO_MINUTE_H; }

        int iTime = (int)time;
        iTime += 30;
        return (DataBase.TimeLimit)iTime;
    }

    /// <summary>
    /// 列挙体内の前の要素を返す
    /// ※先頭を渡した場合末尾が返る
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static TimeLimit GetTimeLimitPrev(TimeLimit time)
    {
        //∞
        //if (time == TimeLimit.INFINITE) { return TimeLimit.TREE_MINUTE; }
        //Head(1分)
        /*else*/ if (time == TimeLimit.ZERO_MINUTE_H) { return TimeLimit.TREE_MINUTE; }

        int iTime = (int)time;
        iTime -= 30;
        return (DataBase.TimeLimit)iTime;
    }
}
/******************************************************************/
/*      制作:IT高度専門学科2年 番場 宥輝   (2018/02/26 現在)      */
/******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePadInput;

/// <summary>
/// ゲームパッドコントローラークラス(XBox対応)
/// </summary>
public class GamePadController : MonoBehaviour {

    private float dead_;

    /// <summary>
    /// パラメータの設定
    /// </summary>
    public void SetParameter(float dead)
    {
        dead_ = dead;
    }

    /// <summary>
    /// コントローラー番号
    /// </summary>
    public GamePadInput.GamePad.Index Controller
    {
        get;
        set;
    }

    /// <summary>
    /// スティック入力
    /// </summary>

    public Vector2 LStick
    {
        get { return GamePad.GetAxis(Controller, GamePad.JoyStick.LEFT); }
    }

    public Vector2 RStick
    {
        get { return GamePad.GetAxis(Controller, GamePad.JoyStick.RIGHT); }
    }

    /// <summary>
    /// トリガー入力
    /// </summary>

    /*
    public float LT
    {
        get { return GamePad.GetTrigger(Controller, GamePad.Trigger.LEFT); }
    }

    public float RT
    {
        get { return GamePad.GetTrigger(Controller, GamePad.Trigger.RIGHT); }
    }
    */
    public bool LT
    {
        get { return GamePad.GetTrigger(Controller, GamePad.Trigger.LEFT) > dead_; }
    }

    public bool RT
    {
        get { return GamePad.GetTrigger(Controller, GamePad.Trigger.RIGHT) < dead_; }
    }

    /// <summary>
    /// 十字キー
    /// </summary>

    public float Arrow_X
    {
        get { return GamePad.GetArrow(Controller, GamePad.Arrow.X); }
    }

    public float Arrow_Y
    {
        get { return GamePad.GetArrow(Controller, GamePad.Arrow.Y); }
    }

    /// <summary>
    /// 押された瞬間
    /// </summary>

    public bool A
    {
        get { return Push_Button(Controller, GamePad.Button.A); }
    }

    public bool B
    {
        get { return Push_Button(Controller, GamePad.Button.B); }
    }

    public bool X
    {
        get { return Push_Button(Controller, GamePad.Button.X); }
    }

    public bool Y
    {
        get { return Push_Button(Controller, GamePad.Button.Y); }
    }

    public bool START
    {
        get { return Push_Button(Controller, GamePad.Button.START); }
    }

    public bool BACK
    {
        get { return Push_Button(Controller, GamePad.Button.BACK); }
    }

    public bool LB
    {
        get { return Push_Button(Controller, GamePad.Button.LB); }
    }

    public bool RB
    {
        get { return Push_Button(Controller, GamePad.Button.RB); }
    }

    /// <summary>
    /// 押している間
    /// </summary>

    public bool A_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.A); }
    }

    public bool B_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.B); }
    }

    public bool X_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.X); }
    }

    public bool Y_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.Y); }
    }

    public bool START_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.START); }
    }

    public bool BACK_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.BACK); }
    }

    public bool LB_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.LB); }
    }

    public bool RB_Hold
    {
        get { return Hold_Button(Controller, GamePad.Button.RB); }
    }

    /// <summary>
    /// 離された瞬間
    /// </summary>

    public bool A_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.A); }
    }

    public bool B_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.B); }
    }

    public bool X_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.X); }
    }

    public bool Y_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.Y); }
    }

    public bool START_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.START); }
    }

    public bool BACK_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.BACK); }
    }

    public bool LB_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.LB); }
    }

    public bool RB_Up
    {
        get { return Pull_Button(Controller, GamePad.Button.RB); }
    }

    /// <summary>
    /// ボタンが押された瞬間
    /// </summary>
    /// <param name="index">プレイヤー番号</param>
    /// <param name="button">ボタン</param>
    private bool Push_Button(GamePad.Index index, GamePad.Button button)
    {
        return GamePad.GetButtonDown(index, button);
    }

    /// <summary>
    /// ボタンが押されている間
    /// </summary>
    /// <param name="index">プレイヤー番号</param>
    /// <param name="button">ボタン</param>
    private bool Hold_Button(GamePad.Index index, GamePad.Button button)
    {
        return GamePad.GetButton(index, button);
    }

    /// <summary>
    /// ボタンが離された瞬間
    /// </summary>
    /// <param name="index">プレイヤー番号</param>
    /// <param name="button">ボタン</param>
    private bool Pull_Button(GamePad.Index index, GamePad.Button button)
    {
        return GamePad.GetButtonUp(index, button);
    }
}

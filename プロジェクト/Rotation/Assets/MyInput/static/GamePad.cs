/******************************************************************/
/*      制作:IT高度専門学科2年 番場 宥輝   (2018/02/26 現在)      */
/******************************************************************/
using UnityEngine;

/// <summary>
/// ゲームパッドの入力補助
/// </summary>
namespace GamePadInput
{
    public static class GamePad 
    {
        /// <summary>
        /// enumプロパティ
        /// </summary>
        public enum Index       { ALL, One, Two, Three, Four }
        public enum Button      { A, B, X, Y, START, BACK, LB, RB, }
        public enum Trigger     { LEFT, RIGHT }
        public enum JoyStick    { LEFT, RIGHT }
        public enum Arrow       { X,Y }
        
        /// <summary>
        /// トリガーの入力情報
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="trigger">トリガー</param>
        /// <returns>入力値</returns>
        public static float GetTrigger(Index index, Trigger trigger)
        {

            string triggerType = "_";

            switch (trigger)
            {

                case Trigger.LEFT:
                    triggerType += "L";
                    break;
                case Trigger.RIGHT:
                    triggerType += "R";
                    break;
            }
            string key = "GamePad" + (int)index + triggerType + "T";
            return Input.GetAxis(key);
        }

        /// <summary>
        /// 矢印キー入力情報
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="arrow">矢印キー</param>
        /// <returns></returns>

        public static float GetArrow(Index index,Arrow arrow)
        {
            string arrowType = "_Button_";

            switch (arrow)
            {
                case Arrow.X:
                    arrowType += "X";
                    break;
                case Arrow.Y:
                    arrowType += "Y";
                    break;
            }
            return Input.GetAxis("GamePad" + (int)index + arrowType);
        }

        /// <summary>
        /// ボタンが押された瞬間
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="button">ボタン</param>
        /// <returns></returns>
        public static bool GetButtonDown(Index index, Button button)
        {
            KeyCode key = GetKeyCode(index, button);
            return Input.GetKeyDown(key);
        }

        /// <summary>
        /// ボタンが押されている間
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="button">ボタン</param>
        /// <returns></returns>
        public static bool GetButton(Index index, Button button)
        {
            KeyCode key = GetKeyCode(index, button);
            return Input.GetKey(key);
        }

        /// <summary>
        /// ボタンが離された瞬間
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="button">ボタン</param>
        /// <returns></returns>
        public static bool GetButtonUp(Index index, Button button)
        {
            KeyCode key = GetKeyCode(index, button);
            return Input.GetKeyUp(key);
        }

        /// <summary>
        /// ジョイスティックの入力
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="joyStick">ジョイスティック</param>
        /// <returns>入力値</returns>
        public static Vector2 GetAxis(Index index, JoyStick joyStick)
        {
            string stickType = "_";

            switch (joyStick)
            {
                case JoyStick.LEFT:
                    stickType += "L";
                    break;
                case JoyStick.RIGHT:
                    stickType += "R";
                    break;
            }

            string key = "GamePad" + (int)index + stickType + "Joystick_";
            Vector2 axis = Vector2.zero;
            axis.x = Input.GetAxis(key + "X");
            axis.y = Input.GetAxis(key + "Y");
            return axis;
        }

        /// <summary>
        /// ボタンに応じたキーコードを返す
        /// </summary>
        /// <param name="index">コントローラー番号</param>
        /// <param name="button">ボタン</param>
        /// <returns>キーコード</returns>
        private static KeyCode GetKeyCode(Index index, Button button)
        {
            switch (index)
            {
                //全員
                case Index.ALL:
                    switch (button)
                    {
                        case Button.A: return KeyCode.JoystickButton0;
                        case Button.B: return KeyCode.JoystickButton1;
                        case Button.X: return KeyCode.JoystickButton2;
                        case Button.Y: return KeyCode.JoystickButton3;
                        case Button.START: return KeyCode.JoystickButton7;
                        case Button.BACK: return KeyCode.JoystickButton6;
                        case Button.LB: return KeyCode.JoystickButton4;
                        case Button.RB: return KeyCode.JoystickButton5;
                    }
                    break;
                //1P
                case Index.One:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick1Button0;
                        case Button.B: return KeyCode.Joystick1Button1;
                        case Button.X: return KeyCode.Joystick1Button2;
                        case Button.Y: return KeyCode.Joystick1Button3;
                        case Button.START: return KeyCode.Joystick1Button7;
                        case Button.BACK: return KeyCode.Joystick1Button6;
                        case Button.LB: return KeyCode.Joystick1Button4;
                        case Button.RB: return KeyCode.Joystick1Button5;
                    }
                    break;
                //2P
                case Index.Two:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick2Button0;
                        case Button.B: return KeyCode.Joystick2Button1;
                        case Button.X: return KeyCode.Joystick2Button2;
                        case Button.Y: return KeyCode.Joystick2Button3;
                        case Button.START: return KeyCode.Joystick2Button7;
                        case Button.BACK: return KeyCode.Joystick2Button6;
                        case Button.LB: return KeyCode.Joystick2Button4;
                        case Button.RB: return KeyCode.Joystick2Button5;
                    }
                    break;
                //3P
                case Index.Three:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick3Button0;
                        case Button.B: return KeyCode.Joystick3Button1;
                        case Button.X: return KeyCode.Joystick3Button2;
                        case Button.Y: return KeyCode.Joystick3Button3;
                        case Button.START: return KeyCode.Joystick3Button7;
                        case Button.BACK: return KeyCode.Joystick3Button6;
                        case Button.LB: return KeyCode.Joystick3Button4;
                        case Button.RB: return KeyCode.Joystick3Button5;
                    }
                    break;
                //4P
                case Index.Four:
                    switch (button)
                    {
                        case Button.A: return KeyCode.Joystick4Button0;
                        case Button.B: return KeyCode.Joystick4Button1;
                        case Button.X: return KeyCode.Joystick4Button2;
                        case Button.Y: return KeyCode.Joystick4Button3;
                        case Button.START: return KeyCode.Joystick4Button7;
                        case Button.BACK: return KeyCode.Joystick4Button6;
                        case Button.LB: return KeyCode.Joystick4Button4;
                        case Button.RB: return KeyCode.Joystick4Button5;
                    }
                    break;
            }
            return KeyCode.None;
        }
    }
}

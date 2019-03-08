/******************************************************************/
/*      制作:IT高度専門学科2年 番場 宥輝   (2018/02/26 現在)      */
/******************************************************************/
using UnityEditor;

/// <summary>
/// https://docs.unity3d.com/ja/540/Manual/class-InputManager.html
/// 参考
/// </summary>
namespace MyInput
{
    /// <summary>
    /// 設定プロパティ群
    /// </summary>
    public struct InputProperty
    {
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity;
        public float dead;
        public float sensitivity;

        public bool snap;
        public bool invert;

        public enum Type
        {
            KeyOrMouseButton,
            MouseMovement,
            JoystickAxis,
        }
        public Type type;

        public int axis;
        public int joyNum;

        public static InputProperty CreateGamePadProperty(string name,float gravity, float dead, float sensitivity, bool snap, bool invert, int axis, int joyNum_)
        {
            InputProperty inputProperty = new InputProperty
            {
                name = name,
                gravity = gravity,
                dead = dead,
                sensitivity = sensitivity,
                invert = invert,
                type = Type.JoystickAxis,
                axis = axis,
                joyNum = joyNum_,
            };

            return inputProperty;
        }
    }

    /// <summary>
    /// InputManager設定クラス
    /// </summary>
    public class InputManagerSetter
    {
        public static InputManagerSetter ims = new InputManagerSetter();

        private static SerializedObject inputObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        private static SerializedProperty axesProperty = inputObject.FindProperty("m_Axes");



        /// <summary>
        ///  設定プロパティの追加
        /// </summary>
        public void AddInputProperty(InputProperty inputProperty)
        {
            axesProperty.arraySize++;
            inputObject.ApplyModifiedProperties();

            SerializedProperty property = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
            GetNextProperty(property, "m_Name").stringValue = inputProperty.name;
            GetNextProperty(property, "descriptiveName").stringValue = inputProperty.descriptiveName;
            GetNextProperty(property, "descriptiveNegativeName").stringValue = inputProperty.descriptiveNegativeName;
            GetNextProperty(property, "negativeButton").stringValue = inputProperty.negativeButton;
            GetNextProperty(property, "positiveButton").stringValue = inputProperty.positiveButton;
            GetNextProperty(property, "altNegativeButton").stringValue = inputProperty.altNegativeButton;
            GetNextProperty(property, "altPositiveButton").stringValue = inputProperty.altPositiveButton;
            GetNextProperty(property, "gravity").floatValue = inputProperty.gravity;
            GetNextProperty(property, "dead").floatValue = inputProperty.dead;
            GetNextProperty(property, "sensitivity").floatValue = inputProperty.sensitivity;
            GetNextProperty(property, "snap").boolValue = inputProperty.snap;
            GetNextProperty(property, "invert").boolValue = inputProperty.invert;
            GetNextProperty(property, "type").intValue = (int)inputProperty.type;
            GetNextProperty(property, "axis").intValue = inputProperty.axis - 1;
            GetNextProperty(property, "joyNum").intValue = inputProperty.joyNum;
        }

        /// <summary>
        /// 次のプロパティを取得
        /// </summary>
        /// <param name="current">現在参照しているプロパティのイテレータ</param>
        /// <param name="name">比較プロパティ名</param>
        /// <returns>NextProperty</returns>
        private SerializedProperty GetNextProperty(SerializedProperty current, string name)
        {
            SerializedProperty property = current.Copy();
            property.Next(true);
            do
            {
                if (property.name == name) return property;
            } while (property.Next(false));
            UnityEngine.Debug.LogError("Error! Not get property");
            return null;
        }

        /// <summary>
        /// 設定済みプロパティのクリア
        /// </summary>
        public void Clear()
        {
            axesProperty.ClearArray();
            inputObject.ApplyModifiedProperties();
        }
    }

    /// <summary>
    /// ゲームパッドのプロパティ設定群
    /// </summary>
    namespace GamePad
    {
        public class GamePadProperty
        {
            public static void PropertySet(float gravity, float dead, float sensitivity, bool joyStickX_invert, bool joyStickY_invert)
            {
                string name = "GamePad";
                for (int i = 0; i < 5; i++)
                {
                    InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + i + "_LJoystick_X", gravity, dead, sensitivity, false, joyStickX_invert, 1, i));
                    InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + i + "_LJoystick_Y", gravity, dead, sensitivity, false, joyStickY_invert, 2, i));
                    InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + i + "_RJoystick_X", gravity, dead, sensitivity, false, joyStickX_invert, 4, i));
                    InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + i + "_RJoystick_Y", gravity, dead, sensitivity, false, joyStickY_invert, 5, i));
                    TriggerProperty(name + i + "_", gravity, dead, sensitivity, i);
                    ButtonProperty(name + i + "_", gravity, dead, sensitivity, i);
                }
            }

            /// <summary>
            /// アナログスティックのXプロパティ設定
            /// </summary>
            private static void L_StickProperty(string name, float gravity, float dead, float sensitivity, bool invert, int index)
            {
                name += "JoyStick_X";
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty("L" + name, gravity, dead, sensitivity, false, invert, 1, index));
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty("R" + name, gravity, dead, sensitivity, false, invert, 4, index));
            }

            /// <summary>
            /// アナログスティックのYプロパティ設定
            /// </summary>
            private static void R_StickProperty(string name, float gravity, float dead, float sensitivity, bool invert, int index)
            {
                name += "JoyStick_Y";
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty("L" + name, gravity, dead, sensitivity, false, invert, 2, index));
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty("R" + name, gravity, dead, sensitivity, false, invert, 5, index));
            }

            /// <summary>
            /// トリガーのプロパティ設定
            /// </summary>
            private static void TriggerProperty(string name, float gravity, float dead, float sensitivity, int index)
            {
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + "LT", dead, gravity, sensitivity, false, false, 3, index));
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + "RT", dead, gravity, sensitivity, false, false, 3, index));
            }

            /// <summary>
            /// ボタンのプロパティ設定
            /// </summary>
            private static void ButtonProperty(string name, float gravity, float dead, float sensitivity, int index)
            {
                name += "Button_";
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + "X", gravity, dead, sensitivity, false, false, 6, index));
                InputManagerSetter.ims.AddInputProperty(InputProperty.CreateGamePadProperty(name + "Y", gravity, dead, sensitivity, false, false, 7, index));
            }
        }

    }
    
}
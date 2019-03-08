/******************************************************************/
/*      制作:IT高度専門学科2年 番場 宥輝   (2018/02/26 現在)      */
/******************************************************************/
using UnityEngine;
using UnityEditor;
using MyInput;
using MyInput.GamePad;

/// <summary>
/// MyInputManagerのエディタ拡張
/// </summary>
[CustomEditor(typeof(MyInputManager))]
public class MyInputManagerEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MyInputManager myInputManager = target as MyInputManager;

        myInputManager.device = (MyInputManager.InputDevice)EditorGUILayout.EnumPopup(new GUIContent("InputDevice", "入力デバイスの種類"), myInputManager.device);

        /************************************************/
        /*  Popupでデバイスに対応した設定項目を表示する */
        /************************************************/
        switch (myInputManager.device)
        {
            //GamePadのプロパティ設定
            case MyInputManager.InputDevice.GamePad:
                {
                    myInputManager.gamePad_gravity      = EditorGUILayout.FloatField(new GUIContent("Gravity", "入力(押したキー) がニュートラルに戻る速さ。"), myInputManager.gamePad_gravity);
                    myInputManager.gamePad_dead         = EditorGUILayout.FloatField(new GUIContent("Dead", "指定した数値未満の正や負の値は 0 として認識されます。"), myInputManager.gamePad_dead);
                    myInputManager.gamePad_sensitivity  = EditorGUILayout.FloatField(new GUIContent("Sensitivity", "キーボード入力の場合、この値を大きくすると、応答時間が速くなります。値が低いと、より滑らかになります。マウス移動の場合、この値によって実際のマウスの移動差分が拡大縮小されます。"), myInputManager.gamePad_sensitivity);
                    myInputManager.joyStickX_invert     = EditorGUILayout.Toggle(new GUIContent("Invert_X", "ジョイスティックの左右の逆転"), myInputManager.joyStickX_invert);
                    myInputManager.joyStickY_invert     = EditorGUILayout.Toggle(new GUIContent("Invert_Y", "ジョイスティックの上下の逆転"), myInputManager.joyStickY_invert);

                    if (GUILayout.Button("Setting on GamePad"))
                    {
                        Debug.Log("InputManagerの設定をゲームパッド用に編集しました");
                        InputManagerSetter.ims.Clear();
                        GamePadProperty.PropertySet(
                            myInputManager.gamePad_gravity,
                            myInputManager.gamePad_dead,
                            myInputManager.gamePad_sensitivity,
                            myInputManager.joyStickX_invert,
                            myInputManager.joyStickY_invert
                            );
                    }
                    break;
                }
            //Keyboardのプロパティ設定
            case MyInputManager.InputDevice.Keyboard:
                {
                    break;
                }

        }

    }
}

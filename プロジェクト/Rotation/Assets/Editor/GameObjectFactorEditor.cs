using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObjectFactor))]
public class GameObjectFactorEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameObjectFactor gameObjectFactor = target as GameObjectFactor;

        if (GUILayout.Button("CreateEmpty"))
        {
            Debug.Log("生成");
            gameObjectFactor.CreateEmpty();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewerManager : SingletonMonoBehaviour<ViewerManager> {

    //private param!
    Viewer[] view;
    [SerializeField] GameObject[] viewerModels;
    [SerializeField] float rotSpeed = 0.8f;

    //Property
    public float RotSpeed { get { return rotSpeed; } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        view = GetComponentsInChildren<Viewer>();
    }

    /// <summary>
    /// ビュー用モデル生成
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="modelIndex"></param>
    public void Create(GamePadInput.GamePad.Index index,int modelIndex)
    {
        view[(int)(index - 1)].Create(viewerModels[modelIndex]);
    }

    public void View()
    {
        foreach(var it in view)
        {
            it.RotateModel();
        }
    }
}

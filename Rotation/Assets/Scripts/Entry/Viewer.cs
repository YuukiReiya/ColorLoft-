using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour {


    [System.NonSerialized] public GameObject viewModel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// ビュー用モデルの生成
    /// </summary>
    /// <param name="model"></param>
    public void Create(GameObject model)
    {
        //既に生成されていたら破棄してから作り直す
        if (viewModel)
        {
            Destroy(viewModel);
        }

        Quaternion rot = Quaternion.Euler(new Vector3(0, 150, 0));
        viewModel = Instantiate(model, this.transform);
        viewModel.transform.rotation = rot;
    }

    /// <summary>
    /// モデルの回転
    /// </summary>
    public void RotateModel()
    {
        if (viewModel)
        {
            Vector3 rot = viewModel.transform.rotation.eulerAngles;
            rot.y += ViewerManager.Instance.RotSpeed;
            viewModel.transform.rotation = Quaternion.Euler(rot);
        }
    }
}

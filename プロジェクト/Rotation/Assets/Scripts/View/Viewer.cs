using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour {


    [System.NonSerialized] private GameObject viewModel;
    private Animator anim;
    private Animator animator
    {
        get
        {
            if (!anim)
            {
                anim = viewModel.GetComponent<Animator>();
            }
            return anim;
        }
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

        //生成
        viewModel = Instantiate(model, this.transform);

        //生成したらカメラを注視させる
        viewModel.transform.LookAt(Camera.main.transform);

        var rot = viewModel.transform.rotation.eulerAngles;
        rot.x = rot.z = 0;
        viewModel.transform.rotation = Quaternion.Euler(rot);
    }

    /// <summary>
    /// モデルの回転
    /// </summary>
    public void RotateModel(float rotSpeed)
    {
        if (!viewModel) { return; }
        Vector3 rot = viewModel.transform.rotation.eulerAngles;
        rot.y += rotSpeed;
        viewModel.transform.rotation = Quaternion.Euler(rot);
    }

    /// <summary>
    /// 正面を向く
    /// (カメラ方向にLookAtしてy軸以外を補正する)
    /// </summary>
    public void LookAtFront()
    {
        var target = Camera.main.transform;
        viewModel.transform.LookAt(target);
        var rot = viewModel.transform.rotation.eulerAngles;
        rot.x = rot.z = 0;
        viewModel.transform.rotation = Quaternion.Euler(rot);
    }

    /// <summary>
    /// 勝利アニメーション
    /// </summary>
    public void StartWinAnimation()
    {
        if (!anim) { anim = animator; }
        anim.SetTrigger("isWin");
    }

    /// <summary>
    /// 敗北アニメーション
    /// </summary>
    public void StartLoseAnimation()
    {
        if (!anim) { anim = animator; }
        anim.SetTrigger("isLose");
    }
}

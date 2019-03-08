using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ViewerManager : SingletonMonoBehaviour<ViewerManager> {

    //private param!
    //Viewer[] view;
    ViewModel[] viewers;
    [SerializeField] GameObject[] viewerModels;
    [SerializeField] float rotSpeed = 0.8f;

    public struct ViewModel
    {
        public Viewer view;
        public bool isRotation;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        var tmp= GetComponentsInChildren<Viewer>();
        //構造体配列は値型のため new いらない？
        viewers = new ViewModel[tmp.Length];

        for(int i = 0; i < tmp.Length; ++i)
        {
            viewers[i].view = tmp[i];
            viewers[i].isRotation = true;
        }
    }

    /// <summary>
    /// ビュー用モデル生成
    /// </summary>
    /// <param name="playerIndex"></param>
    /// <param name="modelNo"></param>
    public void Create(GamePadInput.GamePad.Index index,int modelNo)
    {
        //インデックスのズレをここで修正してあげる
        int viewIndex = (int)(index - 1);
        int modelIndex = modelNo - 1;
        viewers[viewIndex].view.Create(viewerModels[modelIndex]);
    }

    /// <summary>
    /// ビュー用モデルの破棄
    /// </summary>
    public void Destroy(GamePadInput.GamePad.Index index)
    {
        //インデックス
        int viewIndex = (int)(index - 1);

        //破棄するオブジェクトの参照
        var obj = viewers[viewIndex].view.gameObject.transform.GetChild(0).gameObject;//子オブジェクトを取得(子オブジェクトは1つしかないから0)

        //null判定
        if (!obj) { return; }

        //オブジェクト破棄
        Destroy(obj);
    }

    /// <summary>
    /// モデルの回転処理
    /// </summary>
    public void RotateModels()
    {
        //回転対象のモデルを配列で取得
        var view = viewers.Where(it => it.isRotation).ToArray();

        foreach (var it in view)
        {
            it.view.RotateModel(rotSpeed);//モデルの回転
        }
    }

    /// <summary>
    /// モデルのカメラ注視処理
    /// </summary>
    public void LookAtFront()
    {
        //回転対象でないモデルを配列で取得
        var view = viewers.Where(it => !it.isRotation).ToArray();

        foreach(var it in view)
        {
            it.view.LookAtFront();
        }
    }

    public void SetRotationFlags(int index,bool flag)
    {
        viewers[index].isRotation = flag;
    }

    /// <summary>
    /// 勝利アニメーションの開始
    /// </summary>
    /// <param name="index"></param>
    public void StartWinAnimation(int index)
    {
        viewers[index].view.StartWinAnimation();
    }

    /// <summary>
    /// ランクに対応したアニメーション
    /// </summary>
    /// <param name="players"></param>
    /// <param name="ranks"></param>
    public void RankOfAnimation(int[]ranks)
    {
        for(int i = 0; i < ranks.Length; ++i)
        {
            //Debug.Log("添え字番号 = " + i+"/ 順位 = "+ranks[i]);

        }

        for (int i = 0; i < DataBase.PLAYER_NUM; ++i)
        {
            //参加していないプレイヤーは飛ばす
            int id = PlayerManager.Instance.GetID(i);
            if (id == PlayerManager.NOT_ENTRY) continue;

            //各アニメーション分岐
            if (ranks[i] == 1)
            {
                //1位のプレイヤーのみ勝利アニメーション
                //view[i].StartWinAnimation();
                viewers[i].view.StartWinAnimation();
            }
            else
            {
                //1位じゃなければ敗北アニメーション
                //view[i].StartLoseAnimation();
                viewers[i].view.StartLoseAnimation();
            }
        }
    }
}

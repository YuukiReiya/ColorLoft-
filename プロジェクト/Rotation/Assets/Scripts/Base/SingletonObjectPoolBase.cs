/// <summary>
/// 番場 宥輝
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonObjectPoolBase<T> : MonoBehaviour where T : MonoBehaviour
{
    //------Singleton Parameter------//

    //static instance
    private static T instance = null;

    //getter
    public static T Instance
    {
        get
        {
            //null check!
            if (instance == null)
            {
                //find
                instance = (T)FindObjectOfType(typeof(T));

                //not found!
                if (instance == null)
                {
                    Debug.LogError("<color=red>" + typeof(T) + "</color>" + " is nothing");
                }
            }
            return instance;
        }
    }

    //--------Pool Parameter--------//
    [SerializeField] protected GameObject poolObject;   //プールするオブジェクト
    [SerializeField] protected int        poolNum;      //プールに用意しておく初期生成数
    protected List<GameObject>            poolList;     //プールオブジェクトが格納されたリスト

    /// <summary>
    /// 初期化
    /// (ここではプール生成後にSetActive(false)を呼び出している)
    /// </summary>
    public virtual void Initialize()
    {
        //プールの生成
        CreatePool();

        //生成したオブジェクトのフラグをfalseに
        foreach(var it in poolList)
        {
            it.SetActive(false);
        }
    }

    public virtual GameObject GetObject()
    {
        //  リストの中から使用できるオブジェクトを探す
        foreach(var it in poolList)
        {
            //アクティブなものは使用されているため次を判定
            if (it.activeSelf) continue;
            //アクティブがfalseのものはtrueにして返却
            it.SetActive(true);
            return it;
        }

        //リストの中にオブジェクトがなかったため生成する
        var newObj = CreateNewObject();
        poolList.Add(newObj);
        return newObj;
    }

    /// <summary>
    /// プールの生成
    /// </summary>
    protected void CreatePool()
    {
        //リスト作成
        poolList = new List<GameObject>();

        //初期生成数分だけオブジェクトを作る
        for(int i = 0; i < poolNum; ++i)
        {
            poolList.Add(CreateNewObject());
        }
    }

    /// <summary>
    /// インスタンスの生成
    /// </summary>
    /// <returns></returns>
    GameObject CreateNewObject()
    {
        var instance = Instantiate(poolObject);
        instance.name = poolObject.name + " " + (poolList.Count + 1);
        instance.transform.SetParent(transform);
        return instance;
    }
}

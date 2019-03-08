/// <summary>
/// 番場 宥輝
/// </summary>
using UnityEngine;

public class SingletonBase<T> where T : class, new(){

    //static instance
    private static T instance = null;

    //constructor
    protected SingletonBase()
    {
        //replication check!
        if (instance != null)
        {
            UnityEngine.Debug.Log("<color=red>" + typeof(T) + "</color>" + " is being replicated!");
        }
    }

    //getter
    public static T Instance
    {
        get
        {
            //null check!
            if (instance == null)
            {
                //create
                instance = new T();

            }
            return instance;
        }
    }

    //remove reference
    public static void RemoveReference()
    {
        instance = null;
    }

    //create reference
    public static void CreateReference()
    {
        instance = new T();
    }
}

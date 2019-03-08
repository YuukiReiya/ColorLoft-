using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] public class SoundTable : Serialize.TableBase<string, AudioClip, SoundPair> { };
[System.Serializable] public class SoundPair : Serialize.KeyAndValue<string, AudioClip> {

    public SoundPair(string key, AudioClip value) : base(key, value) { }
}

/// <summary>
/// 再生する音のリスト
/// </summary>
public class SoundList : MonoBehaviour {

    [SerializeField] SoundTable table;
    public SoundTable Table { get { return table; } }


    [SerializeField, Tooltip("ONにするとAwake関数でSoundManagerにSoundListを登録する")] bool isDefaultSet = true;

	// Use this for initialization
	void Awake () {

        //リストセット
        if (isDefaultSet)
        {
            SoundManager.Instance.ClearSoundsList();
            SoundManager.Instance.SetSoundList(this);
        }
    }


	
}


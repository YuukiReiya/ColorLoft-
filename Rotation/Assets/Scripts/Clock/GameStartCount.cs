using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameStartCount : SingletonMonoBehaviour<GameStartCount> {

    //  private param!
    [SerializeField] Sprite[] sprites;
    [SerializeField]Image circle;
    [SerializeField]Image count;

    [SerializeField] int scalingFrame = 30;
    [SerializeField] Vector3 scalingVal;

    [SerializeField] int frame = 100;
    int c = 0;

    int index = 0;

    bool isChanging = false;

    //  Property
    public bool isStart { get; private set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator ChageFillAmount()
    {
        while (index < sprites.Length )
        {

            while (circle.fillAmount > 0)
            {
                circle.fillAmount -= 1.0f / frame;
                yield return null;
            }

            //●の縁が無くなった瞬間
            if (index == sprites.Length - 1) { break; }
            count.sprite=sprites[++index];
            circle.fillAmount = 1.0f;
        }
        circle.color = count.color = new Color(0, 0, 0, 0);
        isStart = true;

    }

    public void CountUp()
    {
        StartCoroutine(ChageFillAmount());
    }
}

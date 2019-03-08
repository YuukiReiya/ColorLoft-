using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStartCount : SingletonMonoBehaviour<GameStartCount>
{

    //  private param!
    [SerializeField] Image circle;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] int initCount = 3;
    [SerializeField] int frame = 100;

    //  const param!
    const float COUNT_DOWN_VOLUME = 1.0f;

    //  Property
    public bool isStart { get; private set; }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ChageFillAmount()
    {
        //while (index < sprites.Length )
        //{

        //    while (circle.fillAmount > 0)
        //    {
        //        circle.fillAmount -= 1.0f / frame;
        //        yield return null;
        //    }

        //    //●の縁が無くなった瞬間
        //    if (index == sprites.Length - 1) { break; }
        //    count.sprite=sprites[++index];
        //    circle.fillAmount = 1.0f;
        //}
        //circle.color = count.color = new Color(0, 0, 0, 0);
        //isStart = true;
        int count = initCount;
        countText.text = count.ToString();
        while (count > 0)
        {
            //カウントダウンのカウント音
            SoundManager.Instance.PlayOnSE("CountDown");

            while (circle.fillAmount > 0)
            {
                circle.fillAmount -= 1.0f / frame;
                yield return null;
            }

            //●の縁が無くなった瞬間
            circle.fillAmount = 1.0f;

            //カウンタを減らす
            count--;

            //カウンタを表示
            countText.text = count.ToString();

        }
        circle.color = countText.color = new Color(0, 0, 0, 0);
        isStart = true;

        //カウントダウン終了音
        SoundManager.Instance.PlayOnSE("CountFinish", COUNT_DOWN_VOLUME);

        //ゲーム中のBGM
        SoundManager.Instance.PlayOnBGM("BGM");
    }

    public void CountUp()
    {
        StartCoroutine(ChageFillAmount());
    }

    void PlaySE()
    {
    }
}

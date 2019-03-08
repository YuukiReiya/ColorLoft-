using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メニューの選択に用いる矢印
/// </summary>
public class SelectArrowCursol : MonoBehaviour {

    [SerializeField] Image leftArrowImage;
    [SerializeField] Image rightArrowImage;
    [SerializeField] Color noneSelectColor;
    [SerializeField] Color actionSelectColor;
    [SerializeField] float moveFrame = 15;
    [SerializeField] float movePower = 0.1f;

    public enum Type
    {
        LEFT,   //左
        RIGHT,  //右
        BOTH    //両方
    }

    public void NoneSelect(Type type=Type.BOTH)
    {
        switch (type)
        {
            case Type.LEFT:
                leftArrowImage.color = noneSelectColor;
                break;
            case Type.RIGHT:
                rightArrowImage.color = noneSelectColor;
                break;
            case Type.BOTH:
                leftArrowImage.color = noneSelectColor;
                rightArrowImage.color = noneSelectColor;
                break;
        }

        //Color cr;

        //switch (type)
        //{
        //    case Type.LEFT:
        //        cr = leftArrowImage.color;
        //        cr.a = 0;
        //        leftArrowImage.color = cr;
        //        break;
        //    case Type.RIGHT:
        //        cr = rightArrowImage.color;
        //        cr.a = 0;
        //        rightArrowImage.color = cr;
        //        break;
        //    case Type.BOTH:
        //        cr = leftArrowImage.color;
        //        cr.a = 0;
        //        leftArrowImage.color = cr;

        //        cr = rightArrowImage.color;
        //        cr.a = 0;
        //        rightArrowImage.color = cr;
        //        break;
        //}


    }

    public void Select(Type type = Type.BOTH)
    {
        switch (type)
        {
            case Type.LEFT:
                leftArrowImage.color = actionSelectColor;
                break;
            case Type.RIGHT:
                rightArrowImage.color = actionSelectColor;
                break;
            case Type.BOTH:
                leftArrowImage.color = actionSelectColor;
                rightArrowImage.color = actionSelectColor;
                break;
        }

        //Color cr;

        //switch (type)
        //{
        //    case Type.LEFT:
        //        cr = leftArrowImage.color;
        //        cr.a = 1;
        //        leftArrowImage.color = cr;
        //        break;
        //    case Type.RIGHT:
        //        cr = rightArrowImage.color;
        //        cr.a = 1;
        //        rightArrowImage.color = cr;
        //        break;
        //    case Type.BOTH:
        //        cr = leftArrowImage.color;
        //        cr.a = 1;
        //        leftArrowImage.color = cr;

        //        cr = rightArrowImage.color;
        //        cr.a = 1;
        //        rightArrowImage.color = cr;
        //        break;
        //}

    }

    public void StartMoveArrow()
    {
        StartCoroutine(MoveArrow(leftArrowImage, Vector3.left));
        StartCoroutine(MoveArrow(rightArrowImage, Vector3.right));
    }

    IEnumerator MoveArrow(Image image, Vector3 dir)
    {
        var initPos = image.rectTransform.position;

        while (true)
        {
            //移動
            for(int i = 0; i < moveFrame; ++i)
            {
                var pos = image.rectTransform.position;
                pos += dir * movePower;
                image.rectTransform.position = pos;
                yield return null;
            }

            //移動
            for (int i = 0; i < moveFrame; ++i)
            {
                var pos = image.rectTransform.position;
                pos += dir * movePower*(-1);
                image.rectTransform.position = pos;
                yield return null;
            }

            //位置のリセット
            image.transform.position = initPos;

            yield return null;
        }

    }

}

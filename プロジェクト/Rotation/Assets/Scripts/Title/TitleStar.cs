using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルの星のUI演出
/// </summary>
public class TitleStar : MonoBehaviour {

    //  serialize param!
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] float rotSpeed = 1.0f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] Vector3 center;
    [Header("Width And Height")]
    [SerializeField] float width = 3;
    [SerializeField] float height = 3;

    //  private param!

    // Use this for initialization
    void Start() {

        //  SpriteRendererの取得
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update() {

        RotStar();
    }

    /// <summary>
    /// 星の回転
    /// </summary>
    void RotStar()
    {
        renderer.transform.Rotate(0, 0, rotSpeed);
    }

    IEnumerator Move()
    {
        float x, y, z;
        z = 0;

        while (true)
        {
            float value = Time.time;
            x = Mathf.Cos(value * moveSpeed) * width;
            y = Mathf.Sin(value * moveSpeed) * height;

            transform.position = center + new Vector3(x, y, z);
            yield return null;
        }
    }

}

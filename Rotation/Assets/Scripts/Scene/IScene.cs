using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーンのインターフェース
/// </summary>
public interface IScene
{
    void Start();
    void Update();
    void Destroy();
}
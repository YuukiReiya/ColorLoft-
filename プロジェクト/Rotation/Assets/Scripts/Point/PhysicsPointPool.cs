using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理挙動有のポイントのオブジェクトプール
/// </summary>
public class PhysicsPointPool : SingletonObjectPoolBase<PhysicsPointPool>
{
    private void Awake()
    {
        Initialize();
    }

}

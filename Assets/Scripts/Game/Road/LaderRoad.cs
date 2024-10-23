using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사다리 Road 이동 구현
/// </summary>
public class LaderRoad : Road
{
    [Header("LootAt Obj")]
    [SerializeField] Transform lookObj;
    public Vector3 LookObjPos => lookObj.position;

    private void Start()
    {
        isLaderRoad = true;
    }
}

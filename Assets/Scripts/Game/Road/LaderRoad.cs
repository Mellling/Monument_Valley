using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ٸ� Road �̵� ����
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

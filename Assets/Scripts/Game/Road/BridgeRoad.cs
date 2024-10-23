using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다리를 구성하는 Road 구현을 위한 클래스
/// </summary>
public class BridgeRoad : Road
{
    [Tooltip("A bridge containing this road")]
    [SerializeField] public Bridge bridge;

    private void Start()
    {
        // 다리 Road인지 여부 true로 체크
        isBridgeRoad = true;
    }
}
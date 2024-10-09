using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ٸ��� �����ϴ� Road ������ ���� Ŭ����
/// </summary>
public class BridgeRoad : Road
{
    [Tooltip("A bridge containing this road")]
    [SerializeField] public Bridge bridge;

    private void Start()
    {
        // �ٸ� Road���� ���� true�� üũ
        isBridgeRoad = true;
    }
}
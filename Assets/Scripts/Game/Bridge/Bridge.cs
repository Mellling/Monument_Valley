using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ٸ��� ����� �ڵ��� ��ȣ�ۿ� ���� ���� ������Ʈ
/// </summary>
public class Bridge : MonoBehaviour
{
    [Tooltip("A connected handle")]
    [SerializeField] Handle handle;
    [Tooltip("The roads that the bridge contains")]
    [SerializeField] List<BridgeRoad> bridgeRoads = new List<BridgeRoad>();

    private void Start()
    {
        // BridgeRoad�� ����
        foreach (BridgeRoad road in bridgeRoads)
            road.bridge = this;
    }

    #region Control interactable
    /// <summary>
    /// �ڵ� ���� ���� ���� ������Ʈ�ϴ� �޼��� 
    /// </summary>
    /// <param name="interactable">���� ���� ����</param>
    public void ControlInteractable(bool interactable)
    {
        handle.interactable = interactable;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 다리에 연결된 핸들의 상호작용 가능 여부 업데이트
/// </summary>
public class Bridge : MonoBehaviour
{
    [Tooltip("A connected handle")]
    [SerializeField] Handle handle;
    [Tooltip("The roads that the bridge contains")]
    [SerializeField] List<BridgeRoad> bridgeRoads = new List<BridgeRoad>();

    private void Start()
    {
        // BridgeRoad에 연결
        foreach (BridgeRoad road in bridgeRoads)
            road.bridge = this;
    }

    #region Control interactable
    /// <summary>
    /// 핸들 조작 가능 여부 업데이트하는 메서드 
    /// </summary>
    /// <param name="interactable">조작 가능 여부</param>
    public void ControlInteractable(bool interactable)
    {
        handle.interactable = interactable;
    }
    #endregion
}

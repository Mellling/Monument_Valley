using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 움직일 수 있는 길
/// </summary>
public class Road : MonoBehaviour
{
    [Header("Linked Roads")]
    // Ray의 레이어 마스크
    [SerializeField] LayerMask roadMask;
    [Tooltip("Only on the Road at the end of an object that changes its location through player manipulation")]
    [SerializeField] LayerMask checkLinkedMask;
    // 맵에서 연결된 길을 저장하는 List
    [field : SerializeField] public List<Road> roadLinks { get; private set; } = new List<Road>();

    [Header("Uncertainty Of Connection")]
    public bool isUncertainXZ;
    public bool isUncertainY;

    [Header("Staircase or not")]
    public bool isStair;

    [Header("Last load or not")]
    public bool isEndRoad;

    [Header("Bridge load or not")]
    public bool isBridgeRoad;

    #region Unity Event
    private void Awake()
    {
        // Ray를 쏠 방향을 담아 놓은 배열
        Vector3[] rayDirections =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.up,
            Vector3.down,
            Vector3.right,
            Vector3.left
        };

        foreach (var dir in rayDirections) 
        {
            // dir을 로컬 좌표계에서 월드 좌표계로 변환
            Vector3 worldDir = transform.TransformDirection(dir);

            if (Physics.Raycast(transform.position, worldDir, out var info, 1.3f, roadMask))
            {
                // // GetComponent 호출 전에 Road 컴포넌트 캐싱
                Road road = info.transform.GetComponent<Road>();
                if (road != null && !roadLinks.Contains(road))
                {
                    // 주변에 길이 있다면 List에 추가
                    roadLinks.Add(road);
                }
            }
        }
    }
    #endregion

    #region Update road linked
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (checkLinkedMask.Contain(other.gameObject.layer))
        {
            other.TryGetComponent(out LinkedRoad linked);
            if (isUncertainXZ)
                AddRoad(linked.linkedRoad);
            else if (isUncertainY)
                RemoveRoad(linked.linkedRoad);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (checkLinkedMask.Contain(other.gameObject.layer))
        {
            other.TryGetComponent(out LinkedRoad linked);
            if (isUncertainXZ)
                RemoveRoad(linked.linkedRoad);
            else if (isUncertainY)
                AddRoad(linked.linkedRoad);
        }
    }

    private void AddRoad(Road uncertainRoad)
    {
        if (roadLinks.Contains(uncertainRoad))
            return;

        roadLinks.Add(uncertainRoad);
        uncertainRoad.roadLinks.Add(this);
    }

    private void RemoveRoad(Road uncertainRoad)
    {
        if (!roadLinks.Contains(uncertainRoad))
            return;

        roadLinks.Remove(uncertainRoad);
        uncertainRoad.roadLinks.Remove(this);
    }
    #endregion
}

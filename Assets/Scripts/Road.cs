using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 움직일 수 있는 길
/// </summary>
public class Road : MonoBehaviour
{
    // 맵에서 연결된 길을 저장하는 List
    [SerializeField] public List<Road> roadLinks { get; private set; } = new List<Road>();
    // Ray의 레이어 마스크
    [SerializeField] LayerMask roadMask;

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
            if (Physics.Raycast(transform.position, dir, out var info, 1.3f, roadMask))
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
}

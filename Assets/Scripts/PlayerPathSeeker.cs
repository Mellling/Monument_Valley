using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 플레이어의 현재 위치에서 목적지까지 최단 경로를 찾아 이동하는 클래스
/// </summary>
public class PlayerPathSeeker : MonoBehaviour
{
    [Tooltip("Player game start point road")]
    // 플레이어가 현재 있는 길
    [SerializeField] Road currentRoad;

    #region Player move
    public void Move(Road targetRoad)
    {
        // 목표 길까지의 위치 List를 가져옴
        List<Vector3> path = GetOptimalRoute(targetRoad);

        // 목표 길에 도달할 수 없다면 메소드 종료
        if (path == null) 
            return;

        currentRoad = targetRoad;

        // DOTween 시퀀스를 생성
        Sequence seq = DOTween.Sequence();

        // DOTween을 이용해 이동
        foreach (var road in path)
        {
            // 현재 y값을 유지
            Vector3 targetPosition = new Vector3(road.x, transform.position.y, road.z);
            // 각 위치로 이동 애니메이션을 추가
            seq.Append(transform.DOMove(targetPosition, 0.3f).SetEase(Ease.Linear));
        }
    }
    #endregion

    #region Finding shortest path
    /// <summary>
    /// 최단거리 길 탐색하는 메소드
    /// </summary>
    /// <param name="targetRoad">목표 길 위치</param>
    /// <returns>목표 길까지의 위치 List 또는 null</returns>
    private List<Vector3> GetOptimalRoute(Road targetRoad)
    {
        // 경로 탐색을 위한 Queue 초기화 (path : 탐색 중인 길의 경로/ oldRoad : 이전 길 / serchRoad : 현재 길)
        Queue<(List<Vector3> path, Road oldRoad, Road serchRoad)> pathQueue = new();

        // 현재 도로에서 연결된 도로들을 Queue에 추가
        foreach (Road road in currentRoad.roadLinks)
        {
            pathQueue.Enqueue((new() {currentRoad.transform.position}, currentRoad, road));
        }

        // Queue가 전부 빌 때까지 반복
        while (pathQueue.Count > 0)
        {
            var current = pathQueue.Dequeue();

            // 현재 길이 목표 길과 같다면 경로 반환
            if (current.serchRoad == targetRoad)
            {
                current.path.Add(current.serchRoad.transform.position);
                return current.path;
            }

            // 현재 길에서 연결된 길들 탐색
            foreach (var road in current.serchRoad.roadLinks)
            {
                // 이전 도로로 돌아가지 않도록 검사
                if (road == current.oldRoad)
                    continue;

                // 값 복사 (현재 경로에 현재 길 추가)
                var copyList = current.path.ToList();
                copyList.Add(current.serchRoad.transform.position);

                // 연결된 길을 Queue에 추가
                pathQueue.Enqueue((copyList, current.serchRoad, road));
            }
        }

        // 목표 길에 도달할 수 없다면 null 반환
        return null;
    }
    #endregion
}
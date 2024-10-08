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
        // 목표 길까지의 경로를 List로 가져옴
        List<Road> path = GetOptimalRoute(targetRoad);

        // 목표 길에 도달할 수 없다면 메소드 종료
        if (path == null)
            return;

        currentRoad = targetRoad;

        // DOTween 시퀀스를 생성
        Sequence seq = DOTween.Sequence();

        // DOTween을 이용해 이동
        foreach (var road in path)
        {
            // 현재 도로의 목표 위치를 가져오되, Y값은 현재 위치와 동일하게 설정
            Vector3 targetPosition = new Vector3(road.transform.position.x, transform.position.y, road.transform.position.z);

            // 각 위치로 이동 애니메이션을 추가
            if (road.isStair)
                seq.Append(MoveToRoad(road, targetPosition, 0.6f, 0f));
            else if (road.isEndRoad)
                seq.Append(MoveToRoad(road, targetPosition, yOffset : 0f));
            else
                seq.Append(MoveToRoad(road, targetPosition));
        }
    }

    /// <summary>
    /// 이동 및 회전 처리 메서드
    /// </summary>
    /// <param name="road">이동하고자 하는 길</param>
    /// <param name="targetPosition">플레이어가 이동할 타켓 position</param>
    /// <param name="moveSpeed">플레이어의 이동 속도 (기본값: 0.3f)</param>
    /// <param name="yOffset">Y축으로 이동할 거리. 
    /// 이 값은 플레이어가 길 위에서 어느 정도 떠 있는지(높이)를 조절합니다 (기본값: 0.5f)</param>
    /// <returns>플레이어의 이동을 처리하는 DOTween Tween 객체</returns>
    private Tween MoveToRoad(Road road, Vector3 targetPosition, float moveSpeed = 0.3f, float yOffset = 0.5f)
    {
        // DOTween을 이용해 주어진 길로 이동하는 Tween을 생성
        return transform.DOMove(road.transform.position + Vector3.up * yOffset, moveSpeed).SetEase(Ease.Linear)
            .OnUpdate(() => // 이동하는 동안 매 프레임 호출되는 업데이트 메서드
            {
                // 플레이어의 조작이 길 연결에 영향을 주지 않을 때에만
                if (!road.isUncertainXZ)
                {
                    // 목표 방향으로 회전
                    RotateTowardsTarget(targetPosition);
                }
            });
    }

    /// <summary>
    /// 타겟 방향으로 회전 처리 메서드
    /// </summary>
    /// <param name="targetPosition">플레이어가 이동할 타켓 position</param>
    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        // 목표 위치와 현재 위치 사이의 방향 벡터를 구하고 정규화
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        // Y축 회전만 고려하기 위해 X-Z 평면에서의 방향만 사용
        Vector3 directionOnXZ = new(directionToTarget.x, 0, directionToTarget.z);

        // 방향 벡터가 0이 아닌 경우에만 회전 로직 실행
        if (directionOnXZ != Vector3.zero)
        {

            // 현재 방향과 목표 방향 간의 각도 계산
            float angle = Vector3.Angle(transform.forward, directionOnXZ);

            // 각도가 10도 이상일 경우에만 회전하도록 설정
            if (angle > 10f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionOnXZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);    // 부드럽게 보간하여 회전
            }
        }
    }
    #endregion

    #region Finding shortest path
    /// <summary>
    /// 최단거리 길 탐색하는 메소드
    /// </summary>
    /// <param name="targetRoad">목표 길 위치</param>
    /// <returns>목표 길까지의 위치 List 또는 null</returns>
    private List<Road> GetOptimalRoute(Road targetRoad)
    {
        // 경로 탐색을 위한 Queue 초기화 (path : 탐색 중인 길의 경로/ oldRoad : 이전 길 / serchRoad : 현재 길)
        Queue<(List<Road> path, Road oldRoad, Road serchRoad)> pathQueue = new();

        // 현재 도로에서 연결된 도로들을 Queue에 추가
        foreach (Road road in currentRoad.roadLinks)
        {
            pathQueue.Enqueue((new() { currentRoad }, currentRoad, road));
        }

        // Queue가 전부 빌 때까지 반복
        while (pathQueue.Count > 0)
        {
            var current = pathQueue.Dequeue();

            // 현재 길이 목표 길과 같다면 경로 반환
            if (current.serchRoad == targetRoad)
            {
                current.path.Add(current.serchRoad);
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
                copyList.Add(current.serchRoad);

                // 연결된 길을 Queue에 추가
                pathQueue.Enqueue((copyList, current.serchRoad, road));
            }
        }

        // 목표 길에 도달할 수 없다면 null 반환
        return null;
    }
    #endregion
}
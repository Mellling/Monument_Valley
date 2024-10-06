using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �÷��̾��� ���� ��ġ���� ���������� �ִ� ��θ� ã�� �̵��ϴ� Ŭ����
/// </summary>
public class PlayerPathSeeker : MonoBehaviour
{
    [Tooltip("Player game start point road")]
    // �÷��̾ ���� �ִ� ��
    [SerializeField] Road currentRoad;

    #region Player move
    public void Move(Road targetRoad)
    {
        // ��ǥ ������� ��ġ List�� ������
        List<Vector3> path = GetOptimalRoute(targetRoad);

        // ��ǥ �濡 ������ �� ���ٸ� �޼ҵ� ����
        if (path == null) 
            return;

        currentRoad = targetRoad;

        // DOTween �������� ����
        Sequence seq = DOTween.Sequence();

        // DOTween�� �̿��� �̵�
        foreach (var road in path)
        {
            // ���� y���� ����
            Vector3 targetPosition = new Vector3(road.x, transform.position.y, road.z);
            // �� ��ġ�� �̵� �ִϸ��̼��� �߰�
            seq.Append(transform.DOMove(targetPosition, 0.3f).SetEase(Ease.Linear));
        }
    }
    #endregion

    #region Finding shortest path
    /// <summary>
    /// �ִܰŸ� �� Ž���ϴ� �޼ҵ�
    /// </summary>
    /// <param name="targetRoad">��ǥ �� ��ġ</param>
    /// <returns>��ǥ ������� ��ġ List �Ǵ� null</returns>
    private List<Vector3> GetOptimalRoute(Road targetRoad)
    {
        // ��� Ž���� ���� Queue �ʱ�ȭ (path : Ž�� ���� ���� ���/ oldRoad : ���� �� / serchRoad : ���� ��)
        Queue<(List<Vector3> path, Road oldRoad, Road serchRoad)> pathQueue = new();

        // ���� ���ο��� ����� ���ε��� Queue�� �߰�
        foreach (Road road in currentRoad.roadLinks)
        {
            pathQueue.Enqueue((new() {currentRoad.transform.position}, currentRoad, road));
        }

        // Queue�� ���� �� ������ �ݺ�
        while (pathQueue.Count > 0)
        {
            var current = pathQueue.Dequeue();

            // ���� ���� ��ǥ ��� ���ٸ� ��� ��ȯ
            if (current.serchRoad == targetRoad)
            {
                current.path.Add(current.serchRoad.transform.position);
                return current.path;
            }

            // ���� �濡�� ����� ��� Ž��
            foreach (var road in current.serchRoad.roadLinks)
            {
                // ���� ���η� ���ư��� �ʵ��� �˻�
                if (road == current.oldRoad)
                    continue;

                // �� ���� (���� ��ο� ���� �� �߰�)
                var copyList = current.path.ToList();
                copyList.Add(current.serchRoad.transform.position);

                // ����� ���� Queue�� �߰�
                pathQueue.Enqueue((copyList, current.serchRoad, road));
            }
        }

        // ��ǥ �濡 ������ �� ���ٸ� null ��ȯ
        return null;
    }
    #endregion
}
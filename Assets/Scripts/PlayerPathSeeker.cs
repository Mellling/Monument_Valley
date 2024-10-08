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
        // ��ǥ ������� ��θ� List�� ������
        List<Road> path = GetOptimalRoute(targetRoad);

        // ��ǥ �濡 ������ �� ���ٸ� �޼ҵ� ����
        if (path == null)
            return;

        currentRoad = targetRoad;

        // DOTween �������� ����
        Sequence seq = DOTween.Sequence();

        // DOTween�� �̿��� �̵�
        foreach (var road in path)
        {
            // ���� ������ ��ǥ ��ġ�� ��������, Y���� ���� ��ġ�� �����ϰ� ����
            Vector3 targetPosition = new Vector3(road.transform.position.x, transform.position.y, road.transform.position.z);

            // �� ��ġ�� �̵� �ִϸ��̼��� �߰�
            if (road.isStair)
                seq.Append(MoveToRoad(road, targetPosition, 0.6f, 0f));
            else if (road.isEndRoad)
                seq.Append(MoveToRoad(road, targetPosition, yOffset : 0f));
            else
                seq.Append(MoveToRoad(road, targetPosition));
        }
    }

    /// <summary>
    /// �̵� �� ȸ�� ó�� �޼���
    /// </summary>
    /// <param name="road">�̵��ϰ��� �ϴ� ��</param>
    /// <param name="targetPosition">�÷��̾ �̵��� Ÿ�� position</param>
    /// <param name="moveSpeed">�÷��̾��� �̵� �ӵ� (�⺻��: 0.3f)</param>
    /// <param name="yOffset">Y������ �̵��� �Ÿ�. 
    /// �� ���� �÷��̾ �� ������ ��� ���� �� �ִ���(����)�� �����մϴ� (�⺻��: 0.5f)</param>
    /// <returns>�÷��̾��� �̵��� ó���ϴ� DOTween Tween ��ü</returns>
    private Tween MoveToRoad(Road road, Vector3 targetPosition, float moveSpeed = 0.3f, float yOffset = 0.5f)
    {
        // DOTween�� �̿��� �־��� ��� �̵��ϴ� Tween�� ����
        return transform.DOMove(road.transform.position + Vector3.up * yOffset, moveSpeed).SetEase(Ease.Linear)
            .OnUpdate(() => // �̵��ϴ� ���� �� ������ ȣ��Ǵ� ������Ʈ �޼���
            {
                // �÷��̾��� ������ �� ���ῡ ������ ���� ���� ������
                if (!road.isUncertainXZ)
                {
                    // ��ǥ �������� ȸ��
                    RotateTowardsTarget(targetPosition);
                }
            });
    }

    /// <summary>
    /// Ÿ�� �������� ȸ�� ó�� �޼���
    /// </summary>
    /// <param name="targetPosition">�÷��̾ �̵��� Ÿ�� position</param>
    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        // ��ǥ ��ġ�� ���� ��ġ ������ ���� ���͸� ���ϰ� ����ȭ
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        // Y�� ȸ���� ����ϱ� ���� X-Z ��鿡���� ���⸸ ���
        Vector3 directionOnXZ = new(directionToTarget.x, 0, directionToTarget.z);

        // ���� ���Ͱ� 0�� �ƴ� ��쿡�� ȸ�� ���� ����
        if (directionOnXZ != Vector3.zero)
        {

            // ���� ����� ��ǥ ���� ���� ���� ���
            float angle = Vector3.Angle(transform.forward, directionOnXZ);

            // ������ 10�� �̻��� ��쿡�� ȸ���ϵ��� ����
            if (angle > 10f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionOnXZ);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);    // �ε巴�� �����Ͽ� ȸ��
            }
        }
    }
    #endregion

    #region Finding shortest path
    /// <summary>
    /// �ִܰŸ� �� Ž���ϴ� �޼ҵ�
    /// </summary>
    /// <param name="targetRoad">��ǥ �� ��ġ</param>
    /// <returns>��ǥ ������� ��ġ List �Ǵ� null</returns>
    private List<Road> GetOptimalRoute(Road targetRoad)
    {
        // ��� Ž���� ���� Queue �ʱ�ȭ (path : Ž�� ���� ���� ���/ oldRoad : ���� �� / serchRoad : ���� ��)
        Queue<(List<Road> path, Road oldRoad, Road serchRoad)> pathQueue = new();

        // ���� ���ο��� ����� ���ε��� Queue�� �߰�
        foreach (Road road in currentRoad.roadLinks)
        {
            pathQueue.Enqueue((new() { currentRoad }, currentRoad, road));
        }

        // Queue�� ���� �� ������ �ݺ�
        while (pathQueue.Count > 0)
        {
            var current = pathQueue.Dequeue();

            // ���� ���� ��ǥ ��� ���ٸ� ��� ��ȯ
            if (current.serchRoad == targetRoad)
            {
                current.path.Add(current.serchRoad);
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
                copyList.Add(current.serchRoad);

                // ����� ���� Queue�� �߰�
                pathQueue.Enqueue((copyList, current.serchRoad, road));
            }
        }

        // ��ǥ �濡 ������ �� ���ٸ� null ��ȯ
        return null;
    }
    #endregion
}
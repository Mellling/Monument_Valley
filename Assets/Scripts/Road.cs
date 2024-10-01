using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ ������ �� �ִ� ��
/// </summary>
public class Road : MonoBehaviour
{
    // �ʿ��� ����� ���� �����ϴ� List
    [SerializeField] public List<Road> roadLinks { get; private set; } = new List<Road>();
    // Ray�� ���̾� ����ũ
    [SerializeField] LayerMask roadMask;

    #region Unity Event
    private void Awake()
    {
        // Ray�� �� ������ ��� ���� �迭
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
                // // GetComponent ȣ�� ���� Road ������Ʈ ĳ��
                Road road = info.transform.GetComponent<Road>();
                if (road != null && !roadLinks.Contains(road))
                {
                    // �ֺ��� ���� �ִٸ� List�� �߰�
                    roadLinks.Add(road);
                }
            }
        }
    }
    #endregion
}

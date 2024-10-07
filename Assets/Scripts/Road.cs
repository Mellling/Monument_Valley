using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾ ������ �� �ִ� ��
/// </summary>
public class Road : MonoBehaviour
{
    [Header("Linked Roads")]
    // Ray�� ���̾� ����ũ
    [SerializeField] LayerMask roadMask;
    // �ʿ��� ����� ���� �����ϴ� List
    [field : SerializeField] public List<Road> roadLinks { get; private set; } = new List<Road>();

    [Header("Uncertainty Of Connection")]
    public bool isUncertain;
    [SerializeField] Road uncertainRoad;

    [Header("Uncertainty Of Connection")]
    public bool isStair;

    [Header("Uncertainty Of Connection")]
    public bool isEndRoad;

    public Road UncertainRoad => uncertainRoad;

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
            // dir�� ���� ��ǥ�迡�� ���� ��ǥ��� ��ȯ
            Vector3 worldDir = transform.TransformDirection(dir);

            if (Physics.Raycast(transform.position, worldDir, out var info, 1.3f, roadMask))
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

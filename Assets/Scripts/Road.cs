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
    [Tooltip("Only on the Road at the end of an object that changes its location through player manipulation")]
    [SerializeField] LayerMask checkLinkedMask;
    // �ʿ��� ����� ���� �����ϴ� List
    [field : SerializeField] public List<Road> roadLinks { get; private set; } = new List<Road>();

    [Header("Uncertainty Of Connection")]
    public bool isUncertain;
    [SerializeField] Road uncertainRoad;

    [Header("Staircase or not")]
    public bool isStair;

    [Header("Last load or not")]
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

    #region
    private void OnTriggerEnter(Collider other)
    {
        if (checkLinkedMask.Contain(other.gameObject.layer) && !roadLinks.Contains(uncertainRoad))
        {
            roadLinks.Add(uncertainRoad);
            uncertainRoad.roadLinks.Add(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (checkLinkedMask.Contain(other.gameObject.layer) && roadLinks.Contains(uncertainRoad))
        {
            roadLinks.Remove(uncertainRoad);
            uncertainRoad.roadLinks.Remove(this);
        }
    }
    #endregion
}

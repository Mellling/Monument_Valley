using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ۿ� ���� ������ ���� �Ǵ� ����Ǿ�� �� ��
/// �����ص� Road�� �浹�� Road ��ü���� 
/// �����ϴ� ������ �����ϱ� ���� ������ Ŭ����.
/// </summary>
public class LinkedRoad : MonoBehaviour
{
    [field: SerializeField] public Road linkedRoad { get; private set; }
}

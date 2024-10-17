using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///   �巡�׸� ���� �ٸ��� �ڵ��� ȸ����Ű��, ���� ����� 90���� �����ϴ� ����� ������ Ŭ����
/// </summary>
public class Handle : Rotate, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Rotate object")]
    [SerializeField] Transform bridge;
    [SerializeField] Light handleLight;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] RotationAxis brigdeRotationAxis;
    [SerializeField] RotationAxis handleRotationAxis;
    Sequence rotateSeq;

    [Header("Can interaction")]
    public bool interactable;

    #region Rotate

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!interactable)  // ������ �Ұ����� ������ ���
            return;

        handleLight.enabled = true;

        if (rotateSeq != null && rotateSeq.IsActive())
            rotateSeq.Kill();   // Ȱ�� ������ rotateSeq�� ���� (Kill)
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable)  // ������ �Ұ����� ������ ���
            return;

        float rotationX = eventData.delta.x * rotationSpeed;    // X�� �巡�׷� ���� ȸ���� ���
        float rotationY = eventData.delta.y * rotationSpeed;    // y�� �巡�׷� ���� ȸ���� ���

        // ���� �������� �����ϴ� �ּ� ȸ���� ���� (���� ����)
        if (Mathf.Abs(rotationX) < 0.5f && Mathf.Abs(rotationY) < 0.5f)
            return; // X��� Y�� ��ο��� ���� ���� �������� ��� ȸ���� �������� ����

        // �ٸ� ������Ʈ ȸ��
        RotateObject(rotationX, rotationY, bridge, brigdeRotationAxis);
        // �ڵ� ������Ʈ ȸ��
        RotateObject(rotationX, rotationY, transform, handleRotationAxis);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!interactable)  // ������ �Ұ����� ������ ���
            return;

        handleLight.enabled = false;

        SnapToNearest90Degrees(1f, bridge, brigdeRotationAxis, rotateSeq);
    }
    #endregion
}
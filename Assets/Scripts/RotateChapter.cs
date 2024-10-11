using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateChapter : Rotate, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Rotate object")]
    [SerializeField] RotationAxis rotationAxis;
    [SerializeField] float rotationSpeed = 0.1f;
    Sequence rotateSeq;

    #region Rotate
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (rotateSeq != null && rotateSeq.IsActive())
            rotateSeq.Kill();   // Ȱ�� ������ rotateSeq�� ���� (Kill)
    }

    public void OnDrag(PointerEventData eventData)
    {
        float rotationX = eventData.delta.x * rotationSpeed;    // X�� �巡�׷� ���� ȸ���� ���

        // ���� �������� �����ϴ� �ּ� ȸ���� ���� (���� ����)
        if (Mathf.Abs(rotationX) < 0.5f)
            return; // X�࿡�� ���� ���� �������� ��� ȸ���� �������� ����

        // �ٸ� ������Ʈ ȸ��
        RotateObject(rotationX, 0f, transform, rotationAxis);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToNearest90Degrees(1f, transform, rotationAxis, rotateSeq);
    }
    #endregion
}

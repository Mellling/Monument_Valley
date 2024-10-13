using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    /// <summary>
    /// ������Ʈ ȸ����Ű�� �޼ҵ�
    /// </summary>
    /// <param name="rotationX">X�� �巡�� ȸ����</param>
    /// <param name="rotationY">Y�� �巡�� ȸ����</param>
    /// <param name="rotateObject">ȸ���� ������Ʈ Transform</param>
    /// <param name="rotationAxis">ȸ�� ��</param>
    public void RotateObject(float rotationX, float rotationY, Transform rotateObject, RotationAxis rotationAxis)
    {
        // ���� bridge�� ȸ������ ������
        Vector3 currentRotation = rotateObject.rotation.eulerAngles;  // ���� ������Ʈ�� ȸ������ ���Ϸ� ������ ������

        Quaternion rotationAround;

        // X���� �������� ȸ�� ��� (X��� Y�� �巡�� �ջ�)
        if (rotationAxis.Equals(RotationAxis.right))
        {
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.right);
            // X�� ȸ�� ������ 360�� ���� ���� ����
            currentRotation.x = currentRotation.x % 360f;
        }
        // Y���� �������� ȸ�� ���
        else
        {
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.up);
            // Y�� ȸ�� ������ 360�� ���� ���� ����
            currentRotation.y = currentRotation.y % 360f;
        }

        // ���� rotateObject�� ȸ���� ����� ȸ������ ����
        rotateObject.rotation = rotateObject.rotation * rotationAround;
    }

    /// <summary>
    /// ���� ����� 90���� ������Ʈ �ε巴�� ȸ����Ű�� �޼���
    /// </summary>
    /// <param name="duration">�ִϸ��̼��� �Ϸ�Ǵ� �� �ɸ��� �ð�/param>
    /// <param name="rotateTarget">ȸ����ų ������Ʈ�� Transform</param>
    /// <param name="rotationAxis">ȸ�� ��</param>
    /// <param name="rotateSeq">ȸ�� DOTween ������</param>
    public void SnapToNearest90Degrees(float duration, Transform rotateTarget, RotationAxis rotationAxis, Sequence rotateSeq)
    {
        // ���� rotateTarget�� ȸ������ ������
        Vector3 currentRotation = rotateTarget.rotation.eulerAngles;

        float snap;
        Vector3 targetRotation;

        if (rotationAxis.Equals(RotationAxis.right))
        {
            // X���� 90���� ���� �� �ݿø��Ͽ� ���� ����� 90�� ������ ����
            snap = Mathf.Round(currentRotation.x / 90f) * 90f;
            targetRotation = new(snap, currentRotation.y, currentRotation.z); // Z���� �״�� ����
        }
        else
        {
            // Y���� 90���� ���� �� �ݿø��Ͽ� ���� ����� 90�� ������ ����
            snap = Mathf.Round(currentRotation.y / 90f) * 90f;
            targetRotation = new(currentRotation.x, snap, currentRotation.z); // Z���� �״�� ����
        }

        // DOTween �������� �����Ͽ� �ִϸ��̼��� ����
        rotateSeq = DOTween.Sequence();

        // ������Ʈ�� ȸ���� targetRotation���� ������ ������ duration �ð� ���� �ִϸ��̼�
        // RotateMode.FastBeyond360�� ȸ���� 360���� �Ѿ �� �ֵ��� ���
        rotateSeq.Append(rotateTarget.DORotate(targetRotation, duration, RotateMode.FastBeyond360));
    }
}

/// <summary>
/// ȸ�� ��
/// </summary>
public enum RotationAxis
{
    right,
    up
}

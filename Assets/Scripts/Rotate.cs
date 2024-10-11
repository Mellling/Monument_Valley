using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    /// <summary>
    /// 오브젝트 회전시키는 메소드
    /// </summary>
    /// <param name="rotationX">X축 드래그 회전값</param>
    /// <param name="rotationY">Y축 드래그 회전값</param>
    /// <param name="rotateObject">회전한 오브젝트 Transform</param>
    /// <param name="rotationAxis">회전 축</param>
    public void RotateObject(float rotationX, float rotationY, Transform rotateObject, RotationAxis rotationAxis)
    {
        // 현재 bridge의 회전값을 가져옴
        Vector3 currentRotation = rotateObject.rotation.eulerAngles;  // 현재 오브젝트의 회전값을 오일러 각도로 가져옴

        Quaternion rotationAround;

        // X축을 기준으로 회전 계산 (X축과 Y축 드래그 합산)
        if (rotationAxis.Equals(RotationAxis.right))
        {
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.right);
            // X축 회전 각도를 360도 범위 내로 유지
            currentRotation.x = currentRotation.x % 360f;
        }
        // Y축을 기준으로 회전 계산
        else
        {
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.up);
            // Y축 회전 각도를 360도 범위 내로 유지
            currentRotation.y = currentRotation.y % 360f;
        }

        // 현재 rotateObject의 회전에 계산한 회전값을 누적
        rotateObject.rotation = rotateObject.rotation * rotationAround;
    }

    /// <summary>
    /// 가장 가까운 90도로 오브젝트 부드럽게 회전시키는 메서드
    /// </summary>
    /// <param name="duration">애니메이션이 완료되는 데 걸리는 시간/param>
    /// <param name="rotateTarget">회전시킬 오브젝트의 Transform</param>
    /// <param name="rotationAxis">회전 축</param>
    /// <param name="rotateSeq">회전 DOTween 시퀀스</param>
    public void SnapToNearest90Degrees(float duration, Transform rotateTarget, RotationAxis rotationAxis, Sequence rotateSeq)
    {
        // 현재 rotateTarget의 회전값을 가져옴
        Vector3 currentRotation = rotateTarget.rotation.eulerAngles;

        float snap;
        Vector3 targetRotation;

        if (rotationAxis.Equals(RotationAxis.right))
        {
            // X축을 90도로 나눈 후 반올림하여 가장 가까운 90도 각도로 변경
            snap = Mathf.Round(currentRotation.x / 90f) * 90f;
            targetRotation = new(snap, currentRotation.y, currentRotation.z); // Z축은 그대로 유지
        }
        else
        {
            // Y축을 90도로 나눈 후 반올림하여 가장 가까운 90도 각도로 변경
            snap = Mathf.Round(currentRotation.y / 90f) * 90f;
            targetRotation = new(currentRotation.x, snap, currentRotation.z); // Z축은 그대로 유지
        }

        // DOTween 시퀀스를 생성하여 애니메이션을 정의
        rotateSeq = DOTween.Sequence();

        // 오브젝트의 회전을 targetRotation으로 지정한 값으로 duration 시간 동안 애니메이션
        // RotateMode.FastBeyond360은 회전이 360도를 넘어갈 수 있도록 허용
        rotateSeq.Append(rotateTarget.DORotate(targetRotation, duration, RotateMode.FastBeyond360));
    }
}

/// <summary>
/// 회전 축
/// </summary>
public enum RotationAxis
{
    right,
    up
}

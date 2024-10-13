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
            rotateSeq.Kill();   // 활성 상태인 rotateSeq를 종료 (Kill)
    }

    public void OnDrag(PointerEventData eventData)
    {
        float rotationX = eventData.delta.x * rotationSpeed;    // X축 드래그로 인한 회전값 계산

        // 작은 움직임을 무시하는 최소 회전량 설정 (떨림 방지)
        if (Mathf.Abs(rotationX) < 0.5f)
            return; // X축에서 아주 작은 움직임일 경우 회전을 적용하지 않음

        // 다리 오브젝트 회전
        RotateObject(rotationX, 0f, transform, rotationAxis);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapToNearest90Degrees(1f, transform, rotationAxis, rotateSeq);
    }
    #endregion
}

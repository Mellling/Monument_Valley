using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///   드래그를 통해 다리와 핸들을 회전시키고, 가장 가까운 90도에 스냅하는 기능을 구현한 클래스
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
        if (!interactable)  // 조작이 불가능한 상태일 경우
            return;

        handleLight.enabled = true;

        if (rotateSeq != null && rotateSeq.IsActive())
            rotateSeq.Kill();   // 활성 상태인 rotateSeq를 종료 (Kill)
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!interactable)  // 조작이 불가능한 상태일 경우
            return;

        float rotationX = eventData.delta.x * rotationSpeed;    // X축 드래그로 인한 회전값 계산
        float rotationY = eventData.delta.y * rotationSpeed;    // y축 드래그로 인한 회전값 계산

        // 작은 움직임을 무시하는 최소 회전량 설정 (떨림 방지)
        if (Mathf.Abs(rotationX) < 0.5f && Mathf.Abs(rotationY) < 0.5f)
            return; // X축과 Y축 모두에서 아주 작은 움직임일 경우 회전을 적용하지 않음

        // 다리 오브젝트 회전
        RotateObject(rotationX, rotationY, bridge, brigdeRotationAxis);
        // 핸들 오브젝트 회전
        RotateObject(rotationX, rotationY, transform, handleRotationAxis);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!interactable)  // 조작이 불가능한 상태일 경우
            return;

        handleLight.enabled = false;

        SnapToNearest90Degrees(1f, bridge, brigdeRotationAxis, rotateSeq);
    }
    #endregion
}
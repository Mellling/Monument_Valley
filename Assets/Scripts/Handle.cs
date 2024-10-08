using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///   드래그를 통해 다리와 핸들을 회전시키고, 가장 가까운 90도에 스냅하는 기능을 구현한 클래스
/// </summary>
public class Handle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Rotate bridge")]
    [SerializeField] Transform bridge;
    [SerializeField] Light handleLight;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] RotationAxis brigdeRotationAxis;
    [SerializeField] RotationAxis handleRotationAxis;
    Sequence rotateSeq;

    #region Rotate

    public void OnBeginDrag(PointerEventData eventData)
    {
        handleLight.enabled = true;

        if (rotateSeq != null && rotateSeq.IsActive())
            rotateSeq.Kill();   // 활성 상태인 rotateSeq를 종료 (Kill)
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        handleLight.enabled = false;

        SnapToNearest90Degrees(1f);
    }

    /// <summary>
    /// 오브젝트 회전시키는 메소드
    /// </summary>
    /// <param name="rotationX">X축 드래그 회전값</param>
    /// <param name="rotationY">Y축 드래그 회전값</param>
    /// <param name="rotateObject">회전한 오브젝트 Transform</param>
    /// <param name="rotationAxis">회전 축</param>
    private void RotateObject(float rotationX, float rotationY, Transform rotateObject, RotationAxis rotationAxis)
    {
        // 현재 bridge의 회전값을 가져옴
        Vector3 currentRotation = rotateObject.rotation.eulerAngles;  // 현재 오브젝트의 회전값을 오일러 각도로 가져옴

        Quaternion rotationAround;
        // X축을 기준으로 회전 계산 (X축과 Y축 드래그 합산)
        if (rotationAxis.Equals(RotationAxis.right))
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.right);
        else
            rotationAround = Quaternion.AngleAxis(rotationX + rotationY, Vector3.up);
        // X축 회전 각도를 360도 범위 내로 유지
        currentRotation.x = currentRotation.x % 360f;

        // 현재 bridge의 회전에 계산한 회전값을 누적
        rotateObject.rotation = rotateObject.rotation * rotationAround;
    }

    /// <summary>
    /// 가장 가까운 90도로 다리 오브젝트 부드럽게 회전시키는 메서드
    /// </summary>
    /// <param name="duration">애니메이션이 완료되는 데 걸리는 시간/param>
    private void SnapToNearest90Degrees(float duration)
    {
        // 현재 bridge의 회전값을 가져옴
        Vector3 currentRotation = bridge.rotation.eulerAngles;

        float snap;

        if (brigdeRotationAxis.Equals(RotationAxis.right))
            // X축을 90도로 나눈 후 반올림하여 가장 가까운 90도 각도로 변경
            snap = Mathf.Round(currentRotation.x / 90f) * 90f;
        else
            snap = Mathf.Round(currentRotation.y / 90f) * 90f;


        // 새로운 회전값을 적용
        Vector3 targetRotation = new (snap, currentRotation.y, currentRotation.z); // Z축은 그대로 유지

        // DOTween 시퀀스를 생성하여 애니메이션을 정의
        rotateSeq = DOTween.Sequence();

        // bridge의 회전을 targetRotation으로 지정한 값으로 duration 시간 동안 애니메이션
        // RotateMode.FastBeyond360은 회전이 360도를 넘어갈 수 있도록 허용
        rotateSeq.Append(bridge.DORotate(targetRotation, duration, RotateMode.FastBeyond360));
    }
    #endregion
}

/// <summary>
/// 회전 축
/// </summary>
enum RotationAxis
{
    right,
    up
}
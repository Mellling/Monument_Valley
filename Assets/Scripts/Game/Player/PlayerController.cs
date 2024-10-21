using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 조작 관련 기능 구현 클래스
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Find Player Shortest Distance")]
    [SerializeField] LayerMask roadMask;
    [SerializeField] PlayerPathSeeker pathSeeker;

    [Header("Player and bridge interaction")]
    [SerializeField] bool isOnBridge;
    Bridge collidedBridge;

    private const float rayDistance = 1000f; // Raycast 최대 거리

    [Header("Sound")]
    [SerializeField] AudioClip ClickRoadSFX;

    #region Move
    /// <summary>
    /// 플레이어가 마우스 클릭 시 호출되는 캐릭터 이동 메소드
    /// </summary>
    /// <param name="Value"></param>
    private void OnMove(InputValue Value)
    {
        // 스테이지 종료된 경우
        if (!GameManager.Instance.controlActive)
            return;
        CameraRay();    // 마우스 클릭 시 CameraRay 호출
    }

    /// <summary>
    /// 마우스 클릭 위치로 Ray를 발사하고, 길에 충돌하는지 확인하는 메소드
    /// </summary>
    private void CameraRay()
    {
        // 마우스 클릭 위치로 발사되는 Ray
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast로 길에 충돌하는지 확인
        if (TryGetRoadHit(ray, out Road hitRoad))
        {
            SoundManager.Instance.StopSFX();    // 재생되고 있다는 sfxSource 종료
            SoundManager.Instance.PlaySFX(ClickRoadSFX);    // Road 클릭 사운드 실행
            // 충돌한 오브젝트가 길일 경우 이동 메소드 호출
            pathSeeker.Move(hitRoad);
            CheckBridgeAndUpdateControl(hitRoad);
        }
    }

    /// <summary>
    /// 주어진 Ray로 Raycast를 수행하여 길(Road)에 충돌하는지 확인하는 메소드
    /// </summary>
    /// <param name="ray">발사할 Ray 객체</param>
    /// <param name="hitRoad">충돌한 길(Road) 객체를 반환</param>
    /// <returns> 충돌했으면 true, 아니면 false를 반환</returns>
    private bool TryGetRoadHit(Ray ray, out Road hitRoad)
    {
        hitRoad = null; // 초기화

        // Raycast 수행
        if (Physics.Raycast(ray, out var hitInfo, rayDistance, roadMask))
        {
            // 충돌한 오브젝트에서 Road 컴포넌트를 가져옵니다.
            hitInfo.collider.TryGetComponent(out hitRoad);

            // Road가 null이 아닐 경우 true 반환
            return hitRoad != null;
        }

        return false; // 길에 충돌하지 않음
    }

    /// <summary>
    /// 플레이어와 다리의 상호작용 상태에 따라 조작 가능 여부를 업데이트하는 메서드
    /// </summary>
    /// <param name="clickRoad">플레이어가 클릭한 Road</param>
    private void CheckBridgeAndUpdateControl(Road clickRoad)
    {
        if (clickRoad.isBridgeRoad && !isOnBridge)
        {
            collidedBridge = (clickRoad as BridgeRoad).bridge;
            collidedBridge.ControlInteractable(isOnBridge);
            isOnBridge = true;
        }
        else if (isOnBridge && !clickRoad.isBridgeRoad)
        {
            collidedBridge.ControlInteractable(isOnBridge);
            isOnBridge = false;
        }
    }
    #endregion
}
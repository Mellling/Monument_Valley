using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� ���� ���� ��� ���� Ŭ����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Find Player Shortest Distance")]
    [SerializeField] LayerMask roadMask;
    [SerializeField] PlayerPathSeeker pathSeeker;

    [Header("Player and bridge interaction")]
    [SerializeField] bool isOnBridge;
    Bridge collidedBridge;

    private const float rayDistance = 1000f; // Raycast �ִ� �Ÿ�

    [Header("Sound")]
    [SerializeField] AudioClip ClickRoadSFX;

    #region Move
    /// <summary>
    /// �÷��̾ ���콺 Ŭ�� �� ȣ��Ǵ� ĳ���� �̵� �޼ҵ�
    /// </summary>
    /// <param name="Value"></param>
    private void OnMove(InputValue Value)
    {
        // �������� ����� ���
        if (!GameManager.Instance.controlActive)
            return;
        CameraRay();    // ���콺 Ŭ�� �� CameraRay ȣ��
    }

    /// <summary>
    /// ���콺 Ŭ�� ��ġ�� Ray�� �߻��ϰ�, �濡 �浹�ϴ��� Ȯ���ϴ� �޼ҵ�
    /// </summary>
    private void CameraRay()
    {
        // ���콺 Ŭ�� ��ġ�� �߻�Ǵ� Ray
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast�� �濡 �浹�ϴ��� Ȯ��
        if (TryGetRoadHit(ray, out Road hitRoad))
        {
            SoundManager.Instance.StopSFX();    // ����ǰ� �ִٴ� sfxSource ����
            SoundManager.Instance.PlaySFX(ClickRoadSFX);    // Road Ŭ�� ���� ����
            // �浹�� ������Ʈ�� ���� ��� �̵� �޼ҵ� ȣ��
            pathSeeker.Move(hitRoad);
            CheckBridgeAndUpdateControl(hitRoad);
        }
    }

    /// <summary>
    /// �־��� Ray�� Raycast�� �����Ͽ� ��(Road)�� �浹�ϴ��� Ȯ���ϴ� �޼ҵ�
    /// </summary>
    /// <param name="ray">�߻��� Ray ��ü</param>
    /// <param name="hitRoad">�浹�� ��(Road) ��ü�� ��ȯ</param>
    /// <returns> �浹������ true, �ƴϸ� false�� ��ȯ</returns>
    private bool TryGetRoadHit(Ray ray, out Road hitRoad)
    {
        hitRoad = null; // �ʱ�ȭ

        // Raycast ����
        if (Physics.Raycast(ray, out var hitInfo, rayDistance, roadMask))
        {
            // �浹�� ������Ʈ���� Road ������Ʈ�� �����ɴϴ�.
            hitInfo.collider.TryGetComponent(out hitRoad);

            // Road�� null�� �ƴ� ��� true ��ȯ
            return hitRoad != null;
        }

        return false; // �濡 �浹���� ����
    }

    /// <summary>
    /// �÷��̾�� �ٸ��� ��ȣ�ۿ� ���¿� ���� ���� ���� ���θ� ������Ʈ�ϴ� �޼���
    /// </summary>
    /// <param name="clickRoad">�÷��̾ Ŭ���� Road</param>
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
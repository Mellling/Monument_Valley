using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

/// <summary>
/// 다리 오브젝트 위치 별 사운드 재생을 위한 클래스
/// </summary>
public class BrigeSound : MonoBehaviour
{
    [SerializeField] AudioClip BridgeMoveSFX;
    [SerializeField] LayerMask roadMask;

    /// <summary>
    /// Trigger Enter 시 사운드 재생
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    private void OnTriggerEnter(Collider other)
    {
        if (roadMask.Contain(other.gameObject.layer) && GameManager.Instance.gameStart) // 게임 시작하기 전에는 사운드 플레이 안 함
        {
            SoundManager.Instance.StopSFX();    // 재생되고 있다는 sfxSource 종료
            SoundManager.Instance.PlaySFX(BridgeMoveSFX);   // 다리 사운드 실행
        }
    }

    /// <summary>
    /// Trigger Exit 시 사운드 재생
    /// </summary>
    /// <param name="other">충돌한 콜라이더</param>
    private void OnTriggerExit(Collider other)
    {
        if (roadMask.Contain(other.gameObject.layer) && GameManager.Instance.gameStart) // 게임 시작하기 전에는 사운드 플레이 안 함
        {
            SoundManager.Instance.StopSFX();    // 재생되고 있다는 sfxSource 종료
            SoundManager.Instance.PlaySFX(BridgeMoveSFX);   // 다리 사운드 실행
        }
    }
}

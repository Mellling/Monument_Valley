using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

/// <summary>
/// �ٸ� ������Ʈ ��ġ �� ���� ����� ���� Ŭ����
/// </summary>
public class BrigeSound : MonoBehaviour
{
    [SerializeField] AudioClip BridgeMoveSFX;
    [SerializeField] LayerMask roadMask;

    /// <summary>
    /// Trigger Enter �� ���� ���
    /// </summary>
    /// <param name="other">�浹�� �ݶ��̴�</param>
    private void OnTriggerEnter(Collider other)
    {
        if (roadMask.Contain(other.gameObject.layer) && GameManager.Instance.gameStart) // ���� �����ϱ� ������ ���� �÷��� �� ��
        {
            SoundManager.Instance.StopSFX();    // ����ǰ� �ִٴ� sfxSource ����
            SoundManager.Instance.PlaySFX(BridgeMoveSFX);   // �ٸ� ���� ����
        }
    }

    /// <summary>
    /// Trigger Exit �� ���� ���
    /// </summary>
    /// <param name="other">�浹�� �ݶ��̴�</param>
    private void OnTriggerExit(Collider other)
    {
        if (roadMask.Contain(other.gameObject.layer) && GameManager.Instance.gameStart) // ���� �����ϱ� ������ ���� �÷��� �� ��
        {
            SoundManager.Instance.StopSFX();    // ����ǰ� �ִٴ� sfxSource ����
            SoundManager.Instance.PlaySFX(BridgeMoveSFX);   // �ٸ� ���� ����
        }
    }
}

using UnityEngine;

/// <summary>
/// ���� ���� �� ������ �κ� ��ư ȿ���� ���� Ŭ����
/// </summary>
public class EndGameLobbyButton : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] AudioClip UIClickSFX;

    public void UIClick()
    {
        SoundManager.Instance.PlaySFX(UIClickSFX);
    }
}
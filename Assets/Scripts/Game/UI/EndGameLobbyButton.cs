using UnityEngine;

/// <summary>
/// 게임 끝난 뒤 나오는 로비 버튼 효과를 위한 클래스
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
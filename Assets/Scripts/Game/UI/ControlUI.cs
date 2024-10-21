using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 연결된 UI 켜거나 끄는 기능 구현
/// </summary>
public class ControlUI : MonoBehaviour
{
    [SerializeField] GameObject connectedUI;
    [SerializeField] Button button;

    [Header("Sound")]
    [SerializeField] AudioClip UIClickSFX;

    [Header("UI motion")]
    [SerializeField] CanvasGroup fadeGroup;

    private void Start()
    {
        if (UIManager.Instance.UIHistoryStack.Count == 0)
            UIManager.Instance.UIOpen(gameObject);

        button.onClick.AddListener(() =>
        {
            if (fadeGroup == null)  // Fade 작용이 필요 없을 경우
                UIManager.Instance.UIClose();
            else
                UIManager.Instance.UIClose(fadeGroup);

            UIManager.Instance.UIOpen(connectedUI);
            SoundManager.Instance.PlaySFX(UIClickSFX);
        });
    }

    private void OnEnable()
    {
        UIManager.Instance.FadeIn(fadeGroup);   // 활성화될 때 UIManager의 FadeIn 메서드 호출
    }
}

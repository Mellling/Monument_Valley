using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����� UI �Ѱų� ���� ��� ����
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
        {
            button.onClick.AddListener(() =>
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.controlActive = false;
            });
            UIManager.Instance.UIOpen(gameObject);
        }
        else if (UIManager.Instance.UIHistoryStack.Count == 1)
        {
            button.onClick.AddListener(() =>
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.controlActive = true;
            });
        }

        button.onClick.AddListener(() =>
        {
            if (fadeGroup == null)  // Fade �ۿ��� �ʿ� ���� ���
                UIManager.Instance.UIClose();
            else
                UIManager.Instance.UIClose(fadeGroup);

            UIManager.Instance.UIOpen(connectedUI);
            SoundManager.Instance.PlaySFX(UIClickSFX);
        });
    }

    private void OnEnable()
    {
        UIManager.Instance.FadeIn(fadeGroup);   // Ȱ��ȭ�� �� UIManager�� FadeIn �޼��� ȣ��
    }
}

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
            UIManager.Instance.UIOpen(gameObject);

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

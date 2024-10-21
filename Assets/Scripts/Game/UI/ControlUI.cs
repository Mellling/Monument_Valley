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

    private void Start()
    {
        if (UIManager.Instance.UIHistoryStack.Count == 0)
            UIManager.Instance.UIOpen(gameObject);

        button.onClick.AddListener(() =>
        {
            UIManager.Instance.UIClose();
            UIManager.Instance.UIOpen(connectedUI);
            SoundManager.Instance.PlaySFX(UIClickSFX);
        });
    }
}

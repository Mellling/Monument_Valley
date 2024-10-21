using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 연결된 UI 켜거나 끄는 기능 구현
/// </summary>
public class ControlUI : MonoBehaviour
{
    [SerializeField] GameObject connectedUI;
    [SerializeField] Button button;
    [SerializeField] bool isOpenButton;

    private void Start()
    {
        if (isOpenButton)
        {
            if (UIManager.Instance.UIHistoryStack.Count == 0)
                UIManager.Instance.UIOpen(gameObject);
        }

        button.onClick.AddListener(() =>
        {
            UIManager.Instance.UIClose();
            UIManager.Instance.UIOpen(connectedUI);
        });
    }

    /// <summary>
    /// UI 켜거나 끄는 메서드
    /// </summary>
    public void ControlConnectedUI()
    {
        /*foreach (GameObject ui in connectedUI)
        {
            ui.SetActive(!ui.activeSelf);
        }*/
    }

    public void ControlPlayerActive(bool active)
    {
        GameManager.Instance.controlActive = active;
    }

}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����� UI �Ѱų� ���� ��� ����
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
    /// UI �Ѱų� ���� �޼���
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

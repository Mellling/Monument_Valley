using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����� UI �Ѱų� ���� ��� ����
/// </summary>
public class ControlUI : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedUI;

    /// <summary>
    /// UI �Ѱų� ���� �޼���
    /// </summary>
    public void ControlConnectedUI()
    {
        foreach (GameObject ui in connectedUI)
        {
            ui.SetActive(!ui.activeSelf);
        }
    }

    public void ControlPlayerActive(bool active)
    {
        GameManager.Instance.controlActive = active;
    }

}

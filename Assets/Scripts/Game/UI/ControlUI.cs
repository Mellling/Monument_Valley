using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 연결된 UI 켜거나 끄는 기능 구현
/// </summary>
public class ControlUI : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedUI;

    /// <summary>
    /// UI 켜거나 끄는 메서드
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

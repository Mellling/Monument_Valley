using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI를 관리할 Manager
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    public Stack<GameObject> UIHistoryStack = new Stack<GameObject>();

    #region Unity Event
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion

    #region Open/Colse
    /// <summary>
    /// UI 켜는 메서드
    /// </summary>
    /// <param name="ui">닫을 UI 오브젝트</param>
    public void UIOpen(GameObject ui)
    {
        UIHistoryStack.Push(ui);    // 활성화 할 UI 스택에 Push
        ui.SetActive(true); // UI 오브젝트 활성화
    }

    /// <summary>
    /// UI 닫는 메서드
    /// </summary>
    public void UIClose()
    {
        if (UIHistoryStack.Count == 0)  // 스택 안에 아무 오브젝트도 없을 시
            return;

        UIHistoryStack.Pop().SetActive(false);  // 스택에 있는 오브젝트 꺼내 비활성화
    }
    #endregion
}

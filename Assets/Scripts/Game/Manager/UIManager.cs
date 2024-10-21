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

    public void UIOpen(GameObject ui)
    {
        UIHistoryStack.Push(ui);
        ui.SetActive(true);
        Debug.Log($"Open / {ui.name}");
    }

    public void UIClose()
    {
        if (UIHistoryStack.Count == 0)
            return;
        GameObject obj = UIHistoryStack.Pop();
        obj.SetActive(false);
        Debug.Log($"Close / {obj.name}");
        // UIHistoryStack.Pop().SetActive(true);
    }
}

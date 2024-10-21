using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�� ������ Manager
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
    /// UI �Ѵ� �޼���
    /// </summary>
    /// <param name="ui">���� UI ������Ʈ</param>
    public void UIOpen(GameObject ui)
    {
        UIHistoryStack.Push(ui);    // Ȱ��ȭ �� UI ���ÿ� Push
        ui.SetActive(true); // UI ������Ʈ Ȱ��ȭ
    }

    /// <summary>
    /// UI �ݴ� �޼���
    /// </summary>
    public void UIClose()
    {
        if (UIHistoryStack.Count == 0)  // ���� �ȿ� �ƹ� ������Ʈ�� ���� ��
            return;

        UIHistoryStack.Pop().SetActive(false);  // ���ÿ� �ִ� ������Ʈ ���� ��Ȱ��ȭ
    }
    #endregion
}

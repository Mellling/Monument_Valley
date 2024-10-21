using DG.Tweening;
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

    public void UIClose(CanvasGroup canvasGroup)
    {
        if (UIHistoryStack.Count == 0)  // ���� �ȿ� �ƹ� ������Ʈ�� ���� ��
            return;

        StartFadeOutAndDisable(canvasGroup, UIHistoryStack.Pop());
    }
    #endregion

    #region Smooth motion
    /// <summary>
    /// UI �׷� Fade In ��Ű�� �޼���
    /// </summary>
    /// <param name="canvasGroup">Fade In ��ų canvasGroup</param>
    public void FadeIn(CanvasGroup canvasGroup)
    {
        if (canvasGroup.alpha != 0f)
            canvasGroup.alpha = 0f;

        canvasGroup.DOFade(1f, 0.5f);  // (��ǥ alpha ��, �ִϸ��̼� �ð�)
    }

    /// <summary>
    /// UI �׷� Fade Out ��Ű�� �޼���
    /// </summary>
    /// <param name="canvasGroup">Fade Out ��ų canvasGroup</param>
    /// <returns>FadeOut Tweener ��ȯ</returns>
    public Tweener FadeOut(CanvasGroup canvasGroup)
    {
        return canvasGroup.DOFade(0f, 0.5f);  // (��ǥ alpha ��, �ִϸ��̼� �ð�)
    }

    /// <summary>
    /// FadeOut �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ �۾� �޼���
    /// </summary>
    /// <param name="canvasGroup">Fade Out ��ų canvasGroup</param>
    /// <param name="ui">��Ȱ��ȭ ��ų ������Ʈ</param>
    private void StartFadeOutAndDisable(CanvasGroup canvasGroup, GameObject ui)
    {
        if (canvasGroup != null)
        {
            // FadeOut �ִϸ��̼��� ���� �Ŀ� ������Ʈ�� ��Ȱ��ȭ
            FadeOut(canvasGroup).OnComplete(() => {
                ui.SetActive(false);  // ������Ʈ ��Ȱ��ȭ
            });
        }
    }
    #endregion
}
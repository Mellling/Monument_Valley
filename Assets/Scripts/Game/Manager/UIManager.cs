using DG.Tweening;
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

    public void UIClose(CanvasGroup canvasGroup)
    {
        if (UIHistoryStack.Count == 0)  // 스택 안에 아무 오브젝트도 없을 시
            return;

        StartFadeOutAndDisable(canvasGroup, UIHistoryStack.Pop());
    }
    #endregion

    #region Smooth motion
    /// <summary>
    /// UI 그룹 Fade In 시키는 메서드
    /// </summary>
    /// <param name="canvasGroup">Fade In 시킬 canvasGroup</param>
    public void FadeIn(CanvasGroup canvasGroup)
    {
        if (canvasGroup.alpha != 0f)
            canvasGroup.alpha = 0f;

        canvasGroup.DOFade(1f, 0.5f);  // (목표 alpha 값, 애니메이션 시간)
    }

    /// <summary>
    /// UI 그룹 Fade Out 시키는 메서드
    /// </summary>
    /// <param name="canvasGroup">Fade Out 시킬 canvasGroup</param>
    /// <returns>FadeOut Tweener 반환</returns>
    public Tweener FadeOut(CanvasGroup canvasGroup)
    {
        return canvasGroup.DOFade(0f, 0.5f);  // (목표 alpha 값, 애니메이션 시간)
    }

    /// <summary>
    /// FadeOut 애니메이션 종료 후 오브젝트 비활성화 작업 메서드
    /// </summary>
    /// <param name="canvasGroup">Fade Out 시킬 canvasGroup</param>
    /// <param name="ui">비활성화 시킬 오브젝트</param>
    private void StartFadeOutAndDisable(CanvasGroup canvasGroup, GameObject ui)
    {
        if (canvasGroup != null)
        {
            // FadeOut 애니메이션이 끝난 후에 오브젝트를 비활성화
            FadeOut(canvasGroup).OnComplete(() => {
                ui.SetActive(false);  // 오브젝트 비활성화
            });
        }
    }
    #endregion
}
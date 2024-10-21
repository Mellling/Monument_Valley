using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void FadeIn(Image ui, float targetAlpha)
    {
        if (ui.color.a != 0f)
        {
            Color color = ui.color;
            color.a = 0f;
            ui.color = color;
        }

        StartCoroutine(FadeUI(ui, targetAlpha));
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

    public void FadeOut(Image ui, float targetAlpha)
    {
        StartCoroutine(FadeUI(ui, targetAlpha));
    }

    /// <summary>
    /// FadeOut 애니메이션 종료 후 오브젝트 비활성화 작업 메서드
    /// </summary>
    /// <param name="canvasGroup">Fade Out 시킬 canvasGroup</param>
    /// <param name="ui">비활성화 시킬 오브젝트</param>
    public void StartFadeOutAndDisable(CanvasGroup canvasGroup, GameObject ui)
    {
        if (canvasGroup != null)
        {
            // FadeOut 애니메이션이 끝난 후에 오브젝트를 비활성화
            FadeOut(canvasGroup).OnComplete(() => {
                ui.SetActive(false);  // 오브젝트 비활성화
            });
        }
    }

    /// <summary>
    /// 이미지를 Fade Out시키는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeUI(Image image, float targetAlpha)
    {
        yield return new WaitForSeconds(1.5f);

        Color color = image.color;
        float startAlpha = color.a;  // 초기 알파 값
        float duration = 2.0f;       // 애니메이션 시간 (초)
        float elapsedTime = 0f;      // 경과 시간

        while (elapsedTime < duration)
        {
            float alpha;
            // 경과 시간에 비례하여 0에서 1 사이 값을 반환
            alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            // 알파 값을 업데이트
            color.a = alpha;
            image.color = color;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;  // 한 프레임 대기
        }

        // 애니메이션 종료 후, 알파 값이 정확히 0이 되도록 설정
        color.a = 0.8f;
        image.color = color;
    }
    #endregion
}
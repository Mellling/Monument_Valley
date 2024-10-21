using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// UI �׷� Fade Out ��Ű�� �޼���
    /// </summary>
    /// <param name="canvasGroup">Fade Out ��ų canvasGroup</param>
    /// <returns>FadeOut Tweener ��ȯ</returns>
    public Tweener FadeOut(CanvasGroup canvasGroup)
    {
        return canvasGroup.DOFade(0f, 0.5f);  // (��ǥ alpha ��, �ִϸ��̼� �ð�)
    }

    public void FadeOut(Image ui, float targetAlpha)
    {
        StartCoroutine(FadeUI(ui, targetAlpha));
    }

    /// <summary>
    /// FadeOut �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ �۾� �޼���
    /// </summary>
    /// <param name="canvasGroup">Fade Out ��ų canvasGroup</param>
    /// <param name="ui">��Ȱ��ȭ ��ų ������Ʈ</param>
    public void StartFadeOutAndDisable(CanvasGroup canvasGroup, GameObject ui)
    {
        if (canvasGroup != null)
        {
            // FadeOut �ִϸ��̼��� ���� �Ŀ� ������Ʈ�� ��Ȱ��ȭ
            FadeOut(canvasGroup).OnComplete(() => {
                ui.SetActive(false);  // ������Ʈ ��Ȱ��ȭ
            });
        }
    }

    /// <summary>
    /// �̹����� Fade Out��Ű�� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeUI(Image image, float targetAlpha)
    {
        yield return new WaitForSeconds(1.5f);

        Color color = image.color;
        float startAlpha = color.a;  // �ʱ� ���� ��
        float duration = 2.0f;       // �ִϸ��̼� �ð� (��)
        float elapsedTime = 0f;      // ��� �ð�

        while (elapsedTime < duration)
        {
            float alpha;
            // ��� �ð��� ����Ͽ� 0���� 1 ���� ���� ��ȯ
            alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);

            // ���� ���� ������Ʈ
            color.a = alpha;
            image.color = color;

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null;  // �� ������ ���
        }

        // �ִϸ��̼� ���� ��, ���� ���� ��Ȯ�� 0�� �ǵ��� ����
        color.a = 0.8f;
        image.color = color;
    }
    #endregion
}
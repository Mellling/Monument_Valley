using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Stage �� ���丮 UI ������ ���� Ŭ����
/// </summary>
public class StroyUIControl : MonoBehaviour, IPointerDownHandler
{
    [Header("UI")]
    [SerializeField] Image stroyUIBackground;
    [SerializeField] CanvasGroup stroyUIGroup;

    [SerializeField] TMP_Text stageNum;
    [SerializeField] TMP_Text stageName;
    [SerializeField] TMP_Text stroyText;

    [Header("Sound")]
    [SerializeField] AudioClip ClickUISFX;

    #region unity Event
    private void Start()
    {
        // Text ����
        stageNum.text = GameManager.Instance.stageNum;
        stageName.text = GameManager.Instance.stageName;
        stroyText.text = GameManager.Instance.stageStroy;

        UIManager.Instance.FadeOut(stroyUIBackground, 0.8f);
    }
    #endregion

    #region Mouse Click

    public void OnPointerDown(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(ClickUISFX);
        UIManager.Instance.StartFadeOutAndDisable(stroyUIGroup, stroyUIGroup.gameObject);   // stroyUI ������Ʈ ��ü Fade Out �� ��Ȱ��ȭ
        GameManager.Instance.gameStart = true;
    }
    #endregion
}

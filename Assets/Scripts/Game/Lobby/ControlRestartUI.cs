using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlRestartUI : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] AudioClip ClickStageContinueSFX;

    [Header("For smooth moving")]
    [SerializeField] CanvasGroup restarAllGroup;
    [SerializeField] CanvasGroup restarGroup;

    #region Unity Event
    private void Start()
    {
        UIManager.Instance.FadeIn(restarAllGroup);
    }
    #endregion

    public void WhenButtonClick()
    {
        SoundManager.Instance.PlaySFX(ClickStageContinueSFX);
        UIManager.Instance.FadeOut(restarGroup);
    }
}

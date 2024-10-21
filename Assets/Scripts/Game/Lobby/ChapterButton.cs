using UnityEngine;

/// <summary>
/// 챕터로 이동하는 버튼
/// </summary>
public class ChapterButton : MonoBehaviour
{
    [Tooltip("이동할 챕터의 scene 이름")]
    [SerializeField] string chapterName;
    [SerializeField] string stageNum;
    [SerializeField] string stageName;

    [Header("For smooth moving")]
    [SerializeField] CanvasGroup restarGroup;

    [Header("Sound")]
    [SerializeField] AudioClip ClickChapterButton;

    private void OnMouseDown()
    {
        if (chapterName == null)
            return;

        SoundManager.Instance.PlaySFX(ClickChapterButton);

        LobbyManager.Instance.clickStageName = chapterName;

        if (DataManger.Instance.CheckFileExit(chapterName)) // 스테이지 저장본이 있을 경우
        {
            LobbyManager.Instance.stageNum.text = stageNum;
            LobbyManager.Instance.stageName.text = stageName;
            LobbyManager.Instance.restartUI.SetActive(true);
        }
        else
        {
            DataManger.Instance.needLoadData = false;
            GoToChapter();
        }
    }

    private void GoToChapter()
    {
        // 지정된 챕터의 Scene으로 이동
        LobbyManager.Instance.GoToChapter();
    }
}
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

    private void OnMouseDown()
    {
        if (chapterName == null)
            return;

        LobbyManager.Instance.clickStageName = chapterName;

        if (DataManger.Instance.CheckFileExit(chapterName)) // 스테이지 저장본이 있을 경우
        {
            LobbyManager.Instance.stageNum.text = stageNum;
            LobbyManager.Instance.stageName.text = stageName;
            LobbyManager.Instance.restartUI.SetActive(true);
        }
        else
            GoToChapter();
    }

    private void GoToChapter()
    {
        // 지정된 챕터의 Scene으로 이동
        LobbyManager.Instance.GoToChapter();
    }
}
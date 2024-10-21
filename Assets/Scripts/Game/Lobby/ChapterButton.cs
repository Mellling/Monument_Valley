using UnityEngine;

/// <summary>
/// é�ͷ� �̵��ϴ� ��ư
/// </summary>
public class ChapterButton : MonoBehaviour
{
    [Tooltip("�̵��� é���� scene �̸�")]
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

        if (DataManger.Instance.CheckFileExit(chapterName)) // �������� ���庻�� ���� ���
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
        // ������ é���� Scene���� �̵�
        LobbyManager.Instance.GoToChapter();
    }
}
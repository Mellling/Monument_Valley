using UnityEngine;

/// <summary>
/// 챕터로 이동하는 버튼
/// </summary>
public class ChapterButton : MonoBehaviour
{
    [Tooltip("이동할 챕터의 scene 이름")]
    [SerializeField] string chapterName;

    private void OnMouseDown()
    {
        // 지정된 챕터의 Scene으로 이동
        LobbyManager.Instance.GoToChapter(chapterName);
    }
}
using UnityEngine;

public class ChapterButton : MonoBehaviour
{
    [SerializeField] string chapterName;

    private void OnMouseDown()
    {
        Debug.Log("In");
        LobbyManager.Instance.GoToChapter(chapterName);
    }
}
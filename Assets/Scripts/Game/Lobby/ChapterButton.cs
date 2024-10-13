using UnityEngine;

/// <summary>
/// é�ͷ� �̵��ϴ� ��ư
/// </summary>
public class ChapterButton : MonoBehaviour
{
    [Tooltip("�̵��� é���� scene �̸�")]
    [SerializeField] string chapterName;

    private void OnMouseDown()
    {
        // ������ é���� Scene���� �̵�
        LobbyManager.Instance.GoToChapter(chapterName);
    }
}
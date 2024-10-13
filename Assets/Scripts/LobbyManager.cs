using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby를 관리하는 Manager
/// </summary>
public class LobbyManager : MonoBehaviour
{
    private static LobbyManager instance = new LobbyManager();
    public static LobbyManager Instance => instance;

    #region Unity Event
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    #endregion

    /// <summary>
    /// 챕터가 구현된 Scene으로 이동하는 메서드
    /// </summary>
    /// <param name="chapterName">이동할 챕터의 scene 이름</param>
    public void GoToChapter(string chapterName)
    {
        if (chapterName == null)
            return;
        SceneManager.LoadScene(chapterName);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby�� �����ϴ� Manager
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
    /// é�Ͱ� ������ Scene���� �̵��ϴ� �޼���
    /// </summary>
    /// <param name="chapterName">�̵��� é���� scene �̸�</param>
    public void GoToChapter(string chapterName)
    {
        if (chapterName == null)
            return;
        SceneManager.LoadScene(chapterName);
    }
}

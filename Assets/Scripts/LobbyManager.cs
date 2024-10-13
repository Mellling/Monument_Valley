using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    private static LobbyManager instance = new LobbyManager();
    public static LobbyManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    public void GoToChapter(string chapterName)
    {
        if (chapterName == null)
            return;
        SceneManager.LoadScene(chapterName);
    }
}

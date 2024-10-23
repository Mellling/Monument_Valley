using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Stage �����ϴ� Manager
/// </summary>
public class GameManager : MonoBehaviour
{
    protected static GameManager instance;
    public static GameManager Instance => instance;

    [Header("Player control")]
    public Image block;
    public bool controlActive;
    public bool gameStart;

    [Header("Camera Move")]
    [SerializeField] float cameraMoveDis = 10f;
    public Vector3 cameraTargetPos;
    private bool cameraIsMoving;

    [Header("Go To Lobby")]
    [SerializeField] Button lobbyButton;
    [SerializeField] GameObject openUIButton;

    [Header("UI")]
    [SerializeField] protected GameObject StroyUI;
    public string stageNum;
    public string stageName;
    public string stageStroy;

    [Header("Sound")]
    [SerializeField] protected AudioClip InGameBGM;

    [Header("Data Save&Load")]
    public string chapterName;
    public PlayerPathSeeker pathSeeker;
    public LayerMask roadMask;
    public CanvasGroup loadingUI;

    #region Unity Event
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        controlActive = true;
    }

    private void Start()
    {
        SoundManager.Instance.BGMVolme = SoundManager.Instance.saveBGMVolme;    // é�� �� ���� �� �����ص� BGM ���� �Ҵ�

        if (DataManger.Instance.needLoadData)   // ������ �ε尡 �ʿ��� ��
        {
            loadingUI.gameObject.SetActive(true);
            LoadStageData();
        }
        else
        {
            SoundManager.Instance.PlayBGM(InGameBGM);  // BGM �÷���
            if (StroyUI != null)
                StroyUI.SetActive(true);
            else
                StartCoroutine(WaitGameStart());
        }
    }

    IEnumerator WaitGameStart()
    {
        yield return new WaitForSeconds(1.5f);
        gameStart = true;
    }

    private void Update()
    {
        if (cameraIsMoving) // ī�޶� �������� �ϴ� ������ ���
            CameraMove();   // ī�޶� �����̴� �޼��� ȣ��
    }
    #endregion

    #region Stage End
    /// <summary>
    /// Stage ���� �� ȣ���ϴ� �޼���
    /// </summary>
    public void TheStageEnd()
    {
        // ���� ���� ����
        block.gameObject.SetActive(true);
        controlActive = false;
        openUIButton.SetActive(false);
        // ī�޶��� �̵� cameraTargetPos ����
        cameraTargetPos = Camera.main.transform.position + Vector3.up * cameraMoveDis;
        cameraIsMoving = true;  // ī�޶� �̵� ���� üũ�ϴ� bool ���� true��

        // �������� ������ ����
        File.Delete(DataManger.Instance.FilePath(chapterName));
    }

    /// <summary>
    /// cameraTargetPos���� ī�޶� �̵���Ű�� �޼��� (Update���� ����)
    /// </summary>
    public void CameraMove()
    {
        // ī�޶� cameraTargetPos�� õõ�� �̵�
        Camera.main.transform.position = Vector3.MoveTowards
            (Camera.main.transform.position, cameraTargetPos, 2f * Time.deltaTime);

        // ��ǥ ��ġ ���� Ȯ��
        if (Vector3.Distance(Camera.main.transform.position, cameraTargetPos) < 0.01f)
        {
            cameraIsMoving = false; // ī�޶� �����̴� ���� false�� ��ȯ
            lobbyButton.gameObject.SetActive(true); // �κ�� �̵��ϴ� ��ư ������Ʈ Ȱ��ȭ
        }
    }
    #endregion

    /// <summary>
    /// Lobby Scene���� �̵�
    /// </summary>
    public void GoToLobby()
    {
        StartCoroutine(GoToLobbyRoutine());
    }

    IEnumerator GoToLobbyRoutine()
    {
        float startBGMVolme = SoundManager.Instance.BGMVolme;
        SoundManager.Instance.saveBGMVolme = startBGMVolme;
        float duration = 1.0f;       // �ִϸ��̼� �ð� (��)
        float elapsedTime = 0f;      // ��� �ð�

        while (elapsedTime < duration)
        {
            // ��� �ð��� ����Ͽ� 0���� 1 ���� ���� ��ȯ
            float volme = Mathf.Lerp(startBGMVolme, 0f, elapsedTime / duration);

            SoundManager.Instance.BGMVolme = volme;

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null;  // �� ������ ���
        }

        SoundManager.Instance.StopBGM();    // BGM ����
        UIManager.Instance.UIHistoryStack.Clear();  // UI Stack �ʱ�ȭ
        SceneManager.LoadScene("LobbyScene");
    }

    #region Stage Data
    /// <summary>
    /// �������� ������ ���� �޼���
    /// </summary>
    public virtual void SaveStageData()
    {
        Debug.LogWarning("�������� ���� ���� �޼��尡 �������� �ʾҽ��ϴ�.");
    }

    /// <summary>
    /// �������� ������ �ε� �޼���
    /// </summary>
    public virtual void LoadStageData()
    {
        Debug.LogWarning("�������� ���� �ε� �޼��尡 �������� �ʾҽ��ϴ�.");
    }

    /// <summary>
    /// ��� �ε� ó�� �� �ε� UI ��Ȱ��ȭ �ϱ� ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator CloseLoadingUI()
    {
        yield return null;

    }
    #endregion
}

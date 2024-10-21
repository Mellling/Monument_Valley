using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby�� �����ϴ� Manager
/// </summary>
public class LobbyManager : MonoBehaviour
{
    private static LobbyManager instance;
    public static LobbyManager Instance => instance;

    [Header("Camera Move")]
    [SerializeField] float cameraMoveWaitSec;
    [SerializeField] float cameraMoveDis = 19f;
    public Vector3 cameraTargetPos;
    private bool cameraIsMoving;

    [Header("Block player control")]
    public GameObject restartUI;
    public TMP_Text stageNum;
    public TMP_Text stageName;
    public string clickStageName;

    [Header("Sound")]
    [SerializeField] AudioClip InLobbyBGM;
    // [SerializeField] AudioClip ClickChapter;

    #region Unity Event
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        cameraIsMoving = false;

        // ī�޶��� �̵� cameraTargetPos ����
        cameraTargetPos = Camera.main.transform.position + Vector3.down * cameraMoveDis;
        StartCoroutine(WaitCameraMove()); // ��ٷȴٰ� ī�޶� �����̵���

        SoundManager.Instance.PlayBGM(InLobbyBGM);  // BGM �÷���
    }

    private void Update()
    {
        if (cameraIsMoving) // ī�޶� �������� �ϴ� ������ ���
            CameraMove();   // ī�޶� �����̴� �޼��� ȣ��
    }
    #endregion

    #region When Start Lobby Scene
    /// <summary>
    /// cameraTargetPos���� ī�޶� �̵���Ű�� �޼��� (Update���� ����)
    /// </summary>
    public void CameraMove()
    {
        // ī�޶� cameraTargetPos�� õõ�� �̵�
        Camera.main.transform.position = Vector3.MoveTowards
            (Camera.main.transform.position, cameraTargetPos, 4f * Time.deltaTime);

        // ��ǥ ��ġ ���� Ȯ��
        if (Vector3.Distance(Camera.main.transform.position, cameraTargetPos) < 0.01f)
        {
            cameraIsMoving = false; // ī�޶� �����̴� ���� false�� ��ȯ
        }
    }

    /// <summary>
    /// 3�� �� ī�޶� ���� ���� true�� �ٲٴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitCameraMove()
    {
        yield return new WaitForSeconds(cameraMoveWaitSec);

        cameraIsMoving = true; // ī�޶� �����̴� ���� true�� ��ȯ
    }
    #endregion

    #region Go To Chapter
    public void ClickedContinueButton()
    {
        DataManger.Instance.needLoadData = true;
    }

    public void ClickedRetryButton()
    {
        DataManger.Instance.needLoadData = false;
    }

    /// <summary>
    /// é�Ͱ� ������ Scene���� �̵��ϴ� �޼���
    /// </summary>
    /// <param name="chapterName">�̵��� é���� scene �̸�</param>
    public void GoToChapter()
    {
        if (clickStageName == null)
            return;
        SoundManager.Instance.StopBGM();    // BGM ����
        UIManager.Instance.UIHistoryStack.Clear();  // UI Stack �ʱ�ȭ
        SceneManager.LoadScene($"{clickStageName}Scene");
    }
    #endregion
}

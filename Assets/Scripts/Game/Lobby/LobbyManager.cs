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

    [Header("UI")]
    [SerializeField] GameObject gameMenuUI;

    [Header("Sound")]
    [SerializeField] AudioClip InLobbyBGM;

    #region Unity Event
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(WaitForSoundManager());

        cameraIsMoving = false;

        // ī�޶��� �̵� cameraTargetPos ����
        cameraTargetPos = Camera.main.transform.position + Vector3.down * cameraMoveDis;
        StartCoroutine(WaitCameraMove()); // ��ٷȴٰ� ī�޶� �����̵���
    }

    private void Update()
    {
        if (cameraIsMoving) // ī�޶� �������� �ϴ� ������ ���
            CameraMove();   // ī�޶� �����̴� �޼��� ȣ��
    }
    #endregion

    #region When Start Lobby Scene
    /// <summary>
    /// SoundManager Instance�� �Ҵ� �� �� ���� �۾��� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForSoundManager()
    {
        yield return new WaitUntil(() => SoundManager.Instance != null);

        SoundManager.Instance.BGMVolme = SoundManager.Instance.saveBGMVolme;    // �κ� �� ���� �� �����ص� BGM ���� �Ҵ�
        SoundManager.Instance.PlayBGM(InLobbyBGM);  // BGM �÷���
    }

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
            gameMenuUI.SetActive(true);    // gameMenuUI ������Ʈ Ȱ��ȭ
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
        StartCoroutine(GoToChapterRoutine());
    }

    IEnumerator GoToChapterRoutine()
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
        SceneManager.LoadScene($"{clickStageName}Scene");
    }
    #endregion
}

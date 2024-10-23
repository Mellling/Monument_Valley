using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby를 관리하는 Manager
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

        // 카메라의 이동 cameraTargetPos 설정
        cameraTargetPos = Camera.main.transform.position + Vector3.down * cameraMoveDis;
        StartCoroutine(WaitCameraMove()); // 기다렸다가 카메라 움직이도록
    }

    private void Update()
    {
        if (cameraIsMoving) // 카메라가 움직여야 하는 상태일 경우
            CameraMove();   // 카메라 움직이는 메서드 호출
    }
    #endregion

    #region When Start Lobby Scene
    /// <summary>
    /// SoundManager Instance가 할당 된 후 관련 작업을 실행하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForSoundManager()
    {
        yield return new WaitUntil(() => SoundManager.Instance != null);

        SoundManager.Instance.BGMVolme = SoundManager.Instance.saveBGMVolme;    // 로비 씬 오기 전 조작해둔 BGM 볼륨 할당
        SoundManager.Instance.PlayBGM(InLobbyBGM);  // BGM 플레이
    }

    /// <summary>
    /// cameraTargetPos까지 카메라를 이동시키는 메서드 (Update에서 실행)
    /// </summary>
    public void CameraMove()
    {
        // 카메라 cameraTargetPos로 천천히 이동
        Camera.main.transform.position = Vector3.MoveTowards
            (Camera.main.transform.position, cameraTargetPos, 4f * Time.deltaTime);

        // 목표 위치 도달 확인
        if (Vector3.Distance(Camera.main.transform.position, cameraTargetPos) < 0.01f)
        {
            cameraIsMoving = false; // 카메라 움직이는 여부 false로 전환
            gameMenuUI.SetActive(true);    // gameMenuUI 오브젝트 활성화
        }
    }

    /// <summary>
    /// 3초 후 카메라 무빙 여부 true로 바꾸는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitCameraMove()
    {
        yield return new WaitForSeconds(cameraMoveWaitSec);

        cameraIsMoving = true; // 카메라 움직이는 여부 true로 전환
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
    /// 챕터가 구현된 Scene으로 이동하는 메서드
    /// </summary>
    /// <param name="chapterName">이동할 챕터의 scene 이름</param>
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
        float duration = 1.0f;       // 애니메이션 시간 (초)
        float elapsedTime = 0f;      // 경과 시간

        while (elapsedTime < duration)
        {
            // 경과 시간에 비례하여 0에서 1 사이 값을 반환
            float volme = Mathf.Lerp(startBGMVolme, 0f, elapsedTime / duration);

            SoundManager.Instance.BGMVolme = volme;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;  // 한 프레임 대기
        }

        SoundManager.Instance.StopBGM();    // BGM 정지
        UIManager.Instance.UIHistoryStack.Clear();  // UI Stack 초기화
        SceneManager.LoadScene($"{clickStageName}Scene");
    }
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Stage 관리하는 Manager
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
        SoundManager.Instance.BGMVolme = SoundManager.Instance.saveBGMVolme;    // 챕터 씬 오기 전 조작해둔 BGM 볼륨 할당

        if (DataManger.Instance.needLoadData)   // 데이터 로드가 필요할 시
        {
            loadingUI.gameObject.SetActive(true);
            LoadStageData();
        }
        else
        {
            SoundManager.Instance.PlayBGM(InGameBGM);  // BGM 플레이
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
        if (cameraIsMoving) // 카메라가 움직여야 하는 상태일 경우
            CameraMove();   // 카메라 움직이는 메서드 호출
    }
    #endregion

    #region Stage End
    /// <summary>
    /// Stage 종료 시 호출하는 메서드
    /// </summary>
    public void TheStageEnd()
    {
        // 게임 조작 막기
        block.gameObject.SetActive(true);
        controlActive = false;
        openUIButton.SetActive(false);
        // 카메라의 이동 cameraTargetPos 설정
        cameraTargetPos = Camera.main.transform.position + Vector3.up * cameraMoveDis;
        cameraIsMoving = true;  // 카메라 이동 여부 체크하는 bool 변수 true로

        // 스테이지 데이터 삭제
        File.Delete(DataManger.Instance.FilePath(chapterName));
    }

    /// <summary>
    /// cameraTargetPos까지 카메라를 이동시키는 메서드 (Update에서 실행)
    /// </summary>
    public void CameraMove()
    {
        // 카메라 cameraTargetPos로 천천히 이동
        Camera.main.transform.position = Vector3.MoveTowards
            (Camera.main.transform.position, cameraTargetPos, 2f * Time.deltaTime);

        // 목표 위치 도달 확인
        if (Vector3.Distance(Camera.main.transform.position, cameraTargetPos) < 0.01f)
        {
            cameraIsMoving = false; // 카메라 움직이는 여부 false로 전환
            lobbyButton.gameObject.SetActive(true); // 로비로 이동하는 버튼 오브젝트 활성화
        }
    }
    #endregion

    /// <summary>
    /// Lobby Scene으로 이동
    /// </summary>
    public void GoToLobby()
    {
        StartCoroutine(GoToLobbyRoutine());
    }

    IEnumerator GoToLobbyRoutine()
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
        SceneManager.LoadScene("LobbyScene");
    }

    #region Stage Data
    /// <summary>
    /// 스테이지 데이터 저장 메서드
    /// </summary>
    public virtual void SaveStageData()
    {
        Debug.LogWarning("스테이지 정보 저장 메서드가 구현되지 않았습니다.");
    }

    /// <summary>
    /// 스테이지 데이터 로드 메서드
    /// </summary>
    public virtual void LoadStageData()
    {
        Debug.LogWarning("스테이지 정보 로드 메서드가 구현되지 않았습니다.");
    }

    /// <summary>
    /// 모든 로딩 처리 후 로딩 UI 비활설화 하기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator CloseLoadingUI()
    {
        yield return null;

    }
    #endregion
}

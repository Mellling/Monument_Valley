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

    [Header("Block player control")]
    [SerializeField] Image block;
    private bool isGameEnd;

    [Header("Camera Move")]
    [SerializeField] float cameraMoveDis = 10f;
    public Vector3 cameraTargetPos;
    private bool cameraIsMoving;

    [Header("Go To Lobby")]
    [SerializeField] Button lobbyButton;

    [Header("Save Data")]
    public string stageName;
    public PlayerPathSeeker pathSeeker;
    public LayerMask roadMask;

    public bool IsGameEnd => isGameEnd;

    #region Unity Event
    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (cameraIsMoving) // 카메라가 움직여야 하는 상태일 경우
            CameraMove();   // 카메라 움직이는 메서드 호출
    }
    #endregion

    /// <summary>
    /// Stage 종료 시 호출하는 메서드
    /// </summary>
    public void TheStageEnd()
    {
        // 게임 조작 막기
        block.gameObject.SetActive(true);
        isGameEnd = true;
        // 카메라의 이동 cameraTargetPos 설정
        cameraTargetPos = Camera.main.transform.position + Vector3.up * cameraMoveDis;
        cameraIsMoving = true;  // 카메라 이동 여부 체크하는 bool 변수 true로
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

    /// <summary>
    /// Lobby Scene으로 이동
    /// </summary>
    public void GoToLobby()
    {
        // 스테이지 데이터 삭제
        File.Delete(DataManger.Instance.FilePath(stageName));
        SceneManager.LoadScene("LobbyScene");
    }

    public virtual void SaveStageData()
    {
        Debug.LogWarning("스테이지 정보 저장 메서드가 구현되지 않았습니다.");
    }

    public virtual void LoadStageData()
    {
        Debug.LogWarning("스테이지 정보 로드 메서드가 구현되지 않았습니다.");
    }
}

public enum Stage
{
    stage1 = 1, 
    stage2 = 2
}

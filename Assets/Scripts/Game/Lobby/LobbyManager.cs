using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby를 관리하는 Manager
/// </summary>
public class LobbyManager : MonoBehaviour
{
    private static LobbyManager instance = new LobbyManager();
    public static LobbyManager Instance => instance;

    [Header("Camera Move")]
    [SerializeField] float cameraMoveDis = 21f;
    public Vector3 cameraTargetPos;
    private bool cameraIsMoving = true;

    #region Unity Event
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        // 카메라의 이동 cameraTargetPos 설정
        cameraTargetPos = Camera.main.transform.position + Vector3.down * cameraMoveDis;
    }

    private void Update()
    {
        if (cameraIsMoving) // 카메라가 움직여야 하는 상태일 경우
            CameraMove();   // 카메라 움직이는 메서드 호출
    }
    #endregion

    /// <summary>
    /// cameraTargetPos까지 카메라를 이동시키는 메서드 (Update에서 실행)
    /// </summary>
    public void CameraMove()
    {
        // 카메라 cameraTargetPos로 천천히 이동
        Camera.main.transform.position = Vector3.MoveTowards
            (Camera.main.transform.position, cameraTargetPos, 3f * Time.deltaTime);

        // 목표 위치 도달 확인
        if (Vector3.Distance(Camera.main.transform.position, cameraTargetPos) < 0.01f)
        {
            cameraIsMoving = false; // 카메라 움직이는 여부 false로 전환
        }
    }

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

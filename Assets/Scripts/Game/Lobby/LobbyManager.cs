using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby�� �����ϴ� Manager
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

        // ī�޶��� �̵� cameraTargetPos ����
        cameraTargetPos = Camera.main.transform.position + Vector3.down * cameraMoveDis;
    }

    private void Update()
    {
        if (cameraIsMoving) // ī�޶� �������� �ϴ� ������ ���
            CameraMove();   // ī�޶� �����̴� �޼��� ȣ��
    }
    #endregion

    /// <summary>
    /// cameraTargetPos���� ī�޶� �̵���Ű�� �޼��� (Update���� ����)
    /// </summary>
    public void CameraMove()
    {
        // ī�޶� cameraTargetPos�� õõ�� �̵�
        Camera.main.transform.position = Vector3.MoveTowards
            (Camera.main.transform.position, cameraTargetPos, 3f * Time.deltaTime);

        // ��ǥ ��ġ ���� Ȯ��
        if (Vector3.Distance(Camera.main.transform.position, cameraTargetPos) < 0.01f)
        {
            cameraIsMoving = false; // ī�޶� �����̴� ���� false�� ��ȯ
        }
    }

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

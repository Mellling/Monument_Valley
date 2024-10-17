using System.Collections;
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

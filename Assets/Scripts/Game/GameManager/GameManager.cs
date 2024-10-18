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
        if (cameraIsMoving) // ī�޶� �������� �ϴ� ������ ���
            CameraMove();   // ī�޶� �����̴� �޼��� ȣ��
    }
    #endregion

    /// <summary>
    /// Stage ���� �� ȣ���ϴ� �޼���
    /// </summary>
    public void TheStageEnd()
    {
        // ���� ���� ����
        block.gameObject.SetActive(true);
        isGameEnd = true;
        // ī�޶��� �̵� cameraTargetPos ����
        cameraTargetPos = Camera.main.transform.position + Vector3.up * cameraMoveDis;
        cameraIsMoving = true;  // ī�޶� �̵� ���� üũ�ϴ� bool ���� true��
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

    /// <summary>
    /// Lobby Scene���� �̵�
    /// </summary>
    public void GoToLobby()
    {
        // �������� ������ ����
        File.Delete(DataManger.Instance.FilePath(stageName));
        SceneManager.LoadScene("LobbyScene");
    }

    public virtual void SaveStageData()
    {
        Debug.LogWarning("�������� ���� ���� �޼��尡 �������� �ʾҽ��ϴ�.");
    }

    public virtual void LoadStageData()
    {
        Debug.LogWarning("�������� ���� �ε� �޼��尡 �������� �ʾҽ��ϴ�.");
    }
}

public enum Stage
{
    stage1 = 1, 
    stage2 = 2
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� 1 �����ϴ� Manager
/// </summary>
public class Stage1Manager : GameManager
{
    public Stage1NeedSave needSave;
    [SerializeField] Stage1Data stageData;

    #region Unity Event
    protected override void Awake()
    {
        base.Awake();
        stageName = "Stage1";
    }
    #endregion

    #region Stage Data
    [ContextMenu("Save")]
    public override void SaveStageData()
    {
        if (needSave.IsEverythingAssigned())
        {
            Debug.Log("������ �ʿ��� ������Ʈ �Ҵ� �� ��.");
            return;
        }

        Quaternion playerRotation = needSave.playerTransform.rotation;
        Quaternion bridgeRotation = needSave.bridgeTransform.rotation;
        Quaternion handleRotation = needSave.handleTransform.rotation;

        stageData.SetData(needSave.playerTransform.position, playerRotation, bridgeRotation, handleRotation);

        DataManger.Instance.SaveData(stageData, stageName);
    }

    [ContextMenu("Load")]
    public override void LoadStageData()
    {
        if (needSave.IsEverythingAssigned())
        {
            Debug.Log("������ �ʿ��� ������Ʈ �Ҵ� �� ��.");
            return;
        }

        DataManger.Instance.LoadData(ref stageData, stageName);

        // �÷��̾�
        needSave.playerTransform.position = stageData.playerPos;
        needSave.playerTransform.rotation = stageData.playerRota;

        // �ٸ�
        needSave.bridgeTransform.rotation = stageData.bridgeRota;

        // �ڵ�
        needSave.handleTransform.rotation = stageData.handleRota;

        // PlayerPathSeeker�� currentRoad ����
        if (Physics.Raycast(needSave.playerTransform.position, Vector3.down, out var info, 1.3f, roadMask))
        {
            pathSeeker.currentRoad = info.transform.GetComponent<Road>();
        }
    }
    #endregion
}
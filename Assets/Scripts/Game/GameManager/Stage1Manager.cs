using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지 1 관리하는 Manager
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
            Debug.Log("저장이 필요한 오브젝트 할당 안 됨.");
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
            Debug.Log("저장이 필요한 오브젝트 할당 안 됨.");
            return;
        }

        DataManger.Instance.LoadData(ref stageData, stageName);

        // 플레이어
        needSave.playerTransform.position = stageData.playerPos;
        needSave.playerTransform.rotation = stageData.playerRota;

        // 다리
        needSave.bridgeTransform.rotation = stageData.bridgeRota;

        // 핸들
        needSave.handleTransform.rotation = stageData.handleRota;

        // PlayerPathSeeker의 currentRoad 설정
        if (Physics.Raycast(needSave.playerTransform.position, Vector3.down, out var info, 1.3f, roadMask))
        {
            pathSeeker.currentRoad = info.transform.GetComponent<Road>();
        }
    }
    #endregion
}
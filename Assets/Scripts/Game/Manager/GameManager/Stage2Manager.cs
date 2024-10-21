using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : GameManager
{
    public Stage2NeedSave needSave;
    [SerializeField] Stage2Data stageData;

    #region Unity Event
    protected override void Awake()
    {
        base.Awake();
        chapterName = "Stage2";
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

        stageData.SetData(needSave.playerTransform.position, playerRotation, bridgeRotation, handleRotation,
            needSave.switches[0].isPressed, needSave.switches[1].isPressed);

        DataManger.Instance.SaveData(stageData, chapterName);
    }

    [ContextMenu("Load")]
    public override void LoadStageData()
    {
        if (needSave.IsEverythingAssigned())
        {
            Debug.Log("저장이 필요한 오브젝트 할당 안 됨.");
            return;
        }

        DataManger.Instance.LoadData(ref stageData, chapterName);

        // 플레이어
        needSave.playerTransform.position = stageData.playerPos;
        needSave.playerTransform.rotation = stageData.playerRota;

        // 다리
        needSave.bridgeTransform.rotation = stageData.bridgeRota;

        // 핸들
        needSave.handleTransform.rotation = stageData.handleRota;

        // 스위치
        for (int i = 0; i < stageData.switchActive.Count; i++)
        {
            if (stageData.switchActive[i])
            {
                if (!needSave.switches[i].gameObject.activeSelf)
                    needSave.switches[i].gameObject.SetActive(true);

                needSave.switches[i].WhenSwitchPressed();
            }  
        }

        // PlayerPathSeeker의 currentRoad 설정
        if (Physics.Raycast(needSave.playerTransform.position, Vector3.down, out var info, 1.3f, roadMask))
        {
            pathSeeker.currentRoad = info.transform.GetComponent<Road>();
        }

        StartCoroutine(CloseLoadingUI());
    }

    protected override IEnumerator CloseLoadingUI()
    {
        Debug.Log(stageData.switchActive.Count);
        for (int i = 0; i < stageData.switchActive.Count; i++)
        {
            if (stageData.switchActive[i])
            {
                yield return new WaitUntil(() => needSave.switches[i].isFinished);
            }
        }

        UIManager.Instance.StartFadeOutAndDisable(loadingUI, loadingUI.gameObject);
        StroyUI.SetActive(true);
        SoundManager.Instance.PlayBGM(InGameBGM);  // BGM 플레이
    }
    #endregion
}
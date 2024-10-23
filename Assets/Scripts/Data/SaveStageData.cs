using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지 데이터 Save/Road를 위한 구조체 모음
/// </summary>
public class SaveStageData : MonoBehaviour { }

#region Satge 1
[Serializable]
public struct Stage1Data
{ 
    public Vector3 playerPos;
    public Quaternion playerRota;
    public Quaternion bridgeRota;
    public Quaternion handleRota;

    public void SetData(Vector3 playerPos, Quaternion playerRota, Quaternion bridgeRota, Quaternion handleRota)
    {
        this.playerPos = playerPos;
        this.playerRota = playerRota;
        this.bridgeRota = bridgeRota;
        this.handleRota = handleRota;
    }
}

[Serializable]
public struct Stage1NeedSave
{
    public Transform playerTransform;
    public Transform bridgeTransform;
    public Transform handleTransform;

    public bool IsEverythingAssigned()
    {
        return playerTransform == null || bridgeTransform == null || handleTransform == null;
    }
}
#endregion

#region Stage 2
[Serializable]
public struct Stage2Data
{
    public Vector3 playerPos;
    public Quaternion playerRota;
    public Quaternion bridgeRota;
    public Quaternion handleRota;

    public List<bool> switchActive;

    public void SetData(Vector3 playerPos, Quaternion playerRota, Quaternion bridgeRota, 
        Quaternion handleRota, bool isSwitch1Active, bool isSwitch2Active)
    {
        this .playerPos = playerPos;
        this .playerRota = playerRota;
        this .bridgeRota = bridgeRota;
        this .handleRota = handleRota;

        switchActive.Clear();
        switchActive.Add (isSwitch1Active);
        switchActive.Add (isSwitch2Active);
    }
}

[Serializable]
public struct Stage2NeedSave
{
    public Transform playerTransform;
    public Transform bridgeTransform;
    public Transform handleTransform;
    public List<Switch> switches;

    public bool IsEverythingAssigned()
    {
        return playerTransform == null || bridgeTransform == null || handleTransform == null || switches == null;
    }
}
#endregion

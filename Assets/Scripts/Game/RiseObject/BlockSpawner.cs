using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// List에 담긴 오브젝트를 List 순서대로 부드럽게 위로 상승시키는 기능 구현한 클래스
/// </summary>
public class BlockSpawner : MonoBehaviour
{
    [SerializeField] float riseDis = 2.31f;
    [SerializeField] float riseDuration = 1.0f;
    [Tooltip("Substitute objects in the order in which they are raised")]
    [SerializeField] List<GameObject> objectsToRise = new();
    [SerializeField] List<GameObject> needToActivate = new();

    public bool haveTerm;
    public bool needBlockControl;

    [Header("For Data Load")]
    [SerializeField] Switch connectedSwitch;

    [Header("Sound")]
    [SerializeField] List<AudioClip> riseSFX;

    [ContextMenu("StartRiseObjectsSequentially")]

    /// <summary>
    /// List에 있는 오브젝트들을 순차적으로 이동시키는 코루틴 시작하는 메서드
    /// </summary>
    public void StartRiseObjectsSequentially()
    {
        if (needBlockControl)   // 플레이어의 오브젝트 조작을 막아야 할 경우
            GameManager.Instance.block.gameObject.SetActive(true);
        StartCoroutine(RiseObjectsSequentially());
    }

    /// <summary>
    /// List에 있는 오브젝트들을 순차적으로 이동시키는 코루틴
    /// </summary>
    IEnumerator RiseObjectsSequentially()
    {
        for (int i = 0; i < objectsToRise.Count; i++)
        {
            SoundManager.Instance.PlaySFX(riseSFX[i]);  // 오브젝트 상승 사운드 실행
            if (i == objectsToRise.Count - 1)
                yield return StartCoroutine(RiseObject(objectsToRise[i], () =>
                {
                    connectedSwitch.isFinished = true;
                    if (needBlockControl)
                        GameManager.Instance.block.gameObject.SetActive(false);
                }));
            else
                yield return StartCoroutine(RiseObject(objectsToRise[i]));
            if (haveTerm)
                yield return new WaitForSeconds(riseDuration / 2);
        }

        foreach (GameObject obj in needToActivate)
        {
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// 각각의 오브젝트가 riseDuration 시간 동안 부드럽게 Lerp를 이용해 riseDis만큼 위로 이동하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator RiseObject(GameObject obj)
    {
        Vector3 startPos = obj.transform.position;  // 시작 위치
        Vector3 endPos = startPos + Vector3.up * riseDis;

        float elapsedTime = 0;

        while (elapsedTime < riseDuration)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime/riseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;  // 다음 프레임까지 대기
        }
        obj.transform.position = endPos;  // 정확한 위치에 맞춤
    }

    IEnumerator RiseObject(GameObject obj, Action callback)
    {
        Vector3 startPos = obj.transform.position;  // 시작 위치
        Vector3 endPos = startPos + Vector3.up * riseDis;

        float elapsedTime = 0;

        while (elapsedTime < riseDuration)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / riseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;  // 다음 프레임까지 대기
        }
        obj.transform.position = endPos;  // 정확한 위치에 맞춤

        callback?.Invoke();
    }
}
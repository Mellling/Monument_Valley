using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// List�� ��� ������Ʈ�� List ������� �ε巴�� ���� ��½�Ű�� ��� ������ Ŭ����
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
    /// List�� �ִ� ������Ʈ���� ���������� �̵���Ű�� �ڷ�ƾ �����ϴ� �޼���
    /// </summary>
    public void StartRiseObjectsSequentially()
    {
        if (needBlockControl)   // �÷��̾��� ������Ʈ ������ ���ƾ� �� ���
            GameManager.Instance.block.gameObject.SetActive(true);
        StartCoroutine(RiseObjectsSequentially());
    }

    /// <summary>
    /// List�� �ִ� ������Ʈ���� ���������� �̵���Ű�� �ڷ�ƾ
    /// </summary>
    IEnumerator RiseObjectsSequentially()
    {
        for (int i = 0; i < objectsToRise.Count; i++)
        {
            SoundManager.Instance.PlaySFX(riseSFX[i]);  // ������Ʈ ��� ���� ����
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
    /// ������ ������Ʈ�� riseDuration �ð� ���� �ε巴�� Lerp�� �̿��� riseDis��ŭ ���� �̵��ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator RiseObject(GameObject obj)
    {
        Vector3 startPos = obj.transform.position;  // ���� ��ġ
        Vector3 endPos = startPos + Vector3.up * riseDis;

        float elapsedTime = 0;

        while (elapsedTime < riseDuration)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime/riseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;  // ���� �����ӱ��� ���
        }
        obj.transform.position = endPos;  // ��Ȯ�� ��ġ�� ����
    }

    IEnumerator RiseObject(GameObject obj, Action callback)
    {
        Vector3 startPos = obj.transform.position;  // ���� ��ġ
        Vector3 endPos = startPos + Vector3.up * riseDis;

        float elapsedTime = 0;

        while (elapsedTime < riseDuration)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / riseDuration);
            elapsedTime += Time.deltaTime;
            yield return null;  // ���� �����ӱ��� ���
        }
        obj.transform.position = endPos;  // ��Ȯ�� ��ġ�� ����

        callback?.Invoke();
    }
}
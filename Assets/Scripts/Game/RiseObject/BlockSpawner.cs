using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [ContextMenu("StartRiseObjectsSequentially")]

    /// <summary>
    /// List�� �ִ� ������Ʈ���� ���������� �̵���Ű�� �ڷ�ƾ �����ϴ� �޼���
    /// </summary>
    public void StartRiseObjectsSequentially()
    {
        StartCoroutine(RiseObjectsSequentially());
    }

    /// <summary>
    /// List�� �ִ� ������Ʈ���� ���������� �̵���Ű�� �ڷ�ƾ
    /// </summary>
    IEnumerator RiseObjectsSequentially()
    {
        foreach (GameObject obj in objectsToRise)
        {
            yield return StartCoroutine(RiseObject(obj));
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
}
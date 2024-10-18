using System;
using System.IO;
using UnityEngine;

public class DataManger : MonoBehaviour
{
    private static DataManger instance;
    public static DataManger Instance => instance;

#if UNITY_EDITOR
    private string path = Path.Combine(Application.dataPath, $"Resources/Data/SaveStageData");
#endif

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SaveData<T>(T stageData, string fileName) where T : struct
    {
        string jsonData = JsonUtility.ToJson(stageData);

        string filePath = $"{path}/{fileName}.json";
        // Json ����Ŀ�� ���Ͽ� ����
        File.WriteAllText(filePath, jsonData);
        Debug.Log("������ ���� �Ϸ�");
    }

    public void LoadData<T>(ref T stageData,string fileName) where T : struct
    {
        if (File.Exists($"{path}/{fileName}.json") == false)
            return;

        string jsonData = File.ReadAllText($"{path}/{fileName}.json");

        if (jsonData == null)
            Debug.LogError("Json is null!");

        try
        {
            stageData = JsonUtility.FromJson<T>(jsonData);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Load data fail : {ex.Message}");
        }
    }

    public string FilePath(string fileName)
    {
        return $"{path}/{fileName}.json";
    }

    public bool CheckFileExit(string fileName)
    {
        return File.Exists($"{path}/{fileName}.json");
    }
}

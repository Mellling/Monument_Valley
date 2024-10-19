using System;
using System.IO;
using UnityEngine;

/// <summary>
/// ������ Save/Road ���õ� ���� �����ϴ� Manager
/// </summary>
public class DataManger : MonoBehaviour
{
    private static DataManger instance;
    public static DataManger Instance => instance;

#if UNITY_EDITOR
    private string path = Path.Combine(Application.dataPath, "Resources", "Data", "SaveStageData");
#else
    private string path = Path.Combine(Application.persistentDataPath, "Data", "SaveLoad");
#endif
    // �������� ������ �ε� �ʿ� ����
    public bool needLoadData;


    #region unity Event
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    #endregion

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

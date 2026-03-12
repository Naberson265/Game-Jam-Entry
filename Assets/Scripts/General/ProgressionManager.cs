using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Loading;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public struct CheckpointData : IComparable<CheckpointData>
{
    public string levelScene;
    public Vector3 checkpointPosition;
    
    // Just a number to sort by.
    public int levelNum;
    public int zoneNum;

    public CheckpointData(Vector3 position)
    {
        this.levelScene = SceneManager.GetActiveScene().name;
        this.checkpointPosition = position;
        this.levelNum = GameController.gameController.currentLevel;
        this.zoneNum = GameController.gameController.zone;
    }
    public CheckpointData(Vector3 position, int levelNum, int zoneNum)
    {
        this.levelScene = SceneManager.GetActiveScene().name;
        this.checkpointPosition = position;
        this.levelNum = levelNum;
        this.zoneNum = zoneNum;
    }

    public int CompareTo(CheckpointData other)
    {
        if (zoneNum != other.zoneNum)
        {
            return zoneNum.CompareTo(other.zoneNum);
        }
        // If zones are the same, compare based on level number
        return levelNum.CompareTo(other.levelNum);
    }
}

public class ProgressionManager: MonoBehaviour
{
    [System.Serializable]
    public struct SaveData
    {
        public CheckpointData latestCheckpoint;
        public float[] records;
    }

    public static SaveData _saveData;

    public static ProgressionManager progressionManager { get; private set; }

    private const int RECORDS_PER_LEVEL = 5;
    private const int LEVELS = 6;

    public void Awake()
    {
        if (progressionManager != null && progressionManager != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }
        progressionManager = this;

        // Will persist through scene changes.
        DontDestroyOnLoad(gameObject);

        // Initialize save if no save exists
        if (!File.Exists(SaveFileName()))
        {
            ResetSave();
        }
        Load();
    }

    static public void SaveProgess(Vector3 position)
    {
        CheckpointData newCheckpoint = new CheckpointData(position);

        if (newCheckpoint.CompareTo(_saveData.latestCheckpoint) > 0)
        {
            _saveData.latestCheckpoint = newCheckpoint;
            Save();
        }
    }

    static public void SetRecord(float timerEnd)
    {
        float currentRecord = Mathf.Infinity;
        if (GetCurrentRecord() > 0.1f)
        {
            currentRecord = GetCurrentRecord();
        }

        int levelNum = GameController.gameController.currentLevel;
        int zoneNum = GameController.gameController.zone;
        if (timerEnd<currentRecord)
        {
            _saveData.records[zoneNum * RECORDS_PER_LEVEL + levelNum] = timerEnd;
        }
        Save();
    }

    static public float GetRecord(int levelNum, int zoneNum)
    {
        return _saveData.records[zoneNum * RECORDS_PER_LEVEL + levelNum];
    }

    static public float GetCurrentRecord()
    {
        int levelNum = GameController.gameController.currentLevel;
        int zoneNum = GameController.gameController.zone;
        return GetRecord(levelNum, zoneNum);
    }

    static public void ResetSave()
    {
        _saveData.latestCheckpoint = new CheckpointData(new Vector3(0, 0, 0), -1, -1);
        _saveData.records = new float[RECORDS_PER_LEVEL * LEVELS];
        PlayerPrefs.SetInt("Difficulty", 1);
        Save();
    }

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + "/save" + ".json";
        return saveFile;
    }

    public static void Save()
    {
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData));
        print("Saved");
    }

    public static void Load()
    {
        string saveContent = File.ReadAllText(SaveFileName());

        SaveData saveData = JsonUtility.FromJson<SaveData>(saveContent);
        _saveData = saveData;
        print("Loaded");
    }
}

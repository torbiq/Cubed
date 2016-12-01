using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class PlayerData {
    public uint score = 0;

    public PlayerData() {
        score = 0;
    }
}

public static class DataManager {

    private static string _localFilePath = "/playerData.dat";
    public static string appDataPath { get { return Application.persistentDataPath + _localFilePath; } }

    private static PlayerData _currentData = new PlayerData();
    public static PlayerData currentData { get { return _currentData; } }

    private static uint _bestScore = 0;

    private static uint _BestScore
    {
        get
        {
            return _bestScore;
        }
        set
        {
            _bestScore = value;
            UIManager.UpdateBestScore(value);
        }
    }

    public static uint bestScore
    {
        get { return _bestScore; }
    }

    public static uint playerScore {
        get { return _currentData.score; }
        set {
            if (value > _BestScore) _BestScore = value;
            _currentData.score = value;
        }
    }

    public static void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(appDataPath);

        PlayerData playerDataForSave = new PlayerData();
        playerDataForSave.score = _BestScore;

        bf.Serialize(file, playerDataForSave);

        file.Close();
    }

    public static void Load() {
        if (File.Exists(appDataPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(appDataPath, FileMode.Open);

            PlayerData recievedData = (PlayerData)bf.Deserialize(file);
            _currentData = new PlayerData();
            _BestScore = recievedData.score;
            file.Close();
        }
        else {
            _currentData = new PlayerData();
            _bestScore = 0;
        }
    }

    public static void Init() {
        if (!StateManager.isAppStarted) {
            Load();
        }
        else {
            throw new System.NotImplementedException("Can't initialize date manager more than once");
        }
    }
}

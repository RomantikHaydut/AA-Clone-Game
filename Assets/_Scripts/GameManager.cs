using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool gameOver;

    public static int lastLevel;

    public static bool gameStarted;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }


    [System.Serializable]

    public class SaveData
    {
        public int lastLevel;

        public string playerName;
    }

    public void SaveLevel(int lastlevel)
    {
        SaveData data = new SaveData();

        data.lastLevel = lastlevel;

        string json = JsonUtility.ToJson(data);

        string path = Application.persistentDataPath + "savefile.json";

        File.WriteAllText(path, json);

    }

    public void LoadLevel()
    {
        string path = Application.persistentDataPath + "savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            lastLevel = data.lastLevel;

        }
    }

    public void SavePlayer()
    {

    }

    public void ResetLevel()
    {

    }

    public void ResetPlayer()
    {

    }
}

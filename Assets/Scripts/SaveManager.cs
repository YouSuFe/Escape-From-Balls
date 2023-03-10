using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public int bestScore;
    public string playerName;
    public string currentPlayerName;
    // Start is called before the first frame update
    private void Awake()
    {
        if(Instance == null)
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
    class Save
    {
        public int highScore;
        public string name;
    }

    public void SaveData(int bestWave)
    {
        Save saveData = new Save();
        saveData.highScore = bestWave;
        saveData.name = currentPlayerName;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/saveFile.json", json);
    }

    public void ResetData()
    {
        Save saveData = new Save();
        saveData.highScore = 0;
        saveData.name = "";

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(Application.persistentDataPath + "/SaveFile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/SaveFile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Save saveData = JsonUtility.FromJson<Save>(json);

            bestScore = saveData.highScore;
            playerName = saveData.name;
        }
    }

}

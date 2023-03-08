using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    public TMP_InputField nameField;
    //public Button playButton;
    //public Button resetButton;
    //public Button quitButton;
    public int score;
    public string playerName;
    public string currentPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.LoadData();
        if(SaveManager.Instance.playerName != null)
        {
            playerName = SaveManager.Instance.playerName;
            score = SaveManager.Instance.bestScore;
            bestScoreText.text = "Best Score : " + playerName + " : " + score;
        }
        else
        {
            bestScoreText.text = "GOOD LUCK FOR FIRST GAME";
        }  
    }

    // Update is called once per frame
    void Update()
    {
        UpdateName();
    }

    void UpdateName()
    {
        currentPlayer = nameField.text;
    }

    public void StartGame()
    {
        SaveManager.Instance.playerName = playerName;
        SaveManager.Instance.currentPlayerName = currentPlayer;
        SceneManager.LoadScene(1);
    }

    public void ResetScore()
    {
        if(SaveManager.Instance.playerName != null)
        {
            SaveManager.Instance.ResetData();
            SaveManager.Instance.LoadData();
            bestScoreText.text = "Best Score Reseted!";
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

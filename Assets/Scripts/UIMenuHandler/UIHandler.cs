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
    public int score;
    public string playerName;
    public string currentPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Instance.LoadData();
        UpdateMenuText();
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

    void UpdateMenuText()
    {
        if (SaveManager.Instance.bestScore != 0)
        {
            playerName = SaveManager.Instance.playerName;
            score = SaveManager.Instance.bestScore;
            bestScoreText.text = "Best Score : " + playerName + " : " + score;
        }
        else
        {
            bestScoreText.text = "GOOD LUCK FOR THE FIRST GAME";
        }
    }

    public void StartGame()
    {
        SaveManager.Instance.playerName = playerName;
        SaveManager.Instance.currentPlayerName = currentPlayer;
        SceneManager.LoadScene(1);
    }

    public void ResetScore()
    {
        if(SaveManager.Instance.bestScore != 0)
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

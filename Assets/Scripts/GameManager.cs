using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerUpPrefab;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public TextMeshProUGUI bestScoreTextPanel;
    public TextMeshProUGUI bestScoreText;
    private PlayerController playerCont;

    public string playerName;
    public string currentPlayer;
    public int bestScore = 0;
    
    private float spawnRange = -9;
    private int waveNumber = 1;
    private int score;
    [SerializeField] private int enemyCount;
    [SerializeField] private bool isMenuActive;
    // Start is called before the first frame update
    void Start()
    {
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();

        EnemySpawnWave(waveNumber);
        Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
        UpdatePlayer();

    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = GameManager.FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0 && !playerCont.isGameOver)
        {
            waveNumber++;
            Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
            EnemySpawnWave(waveNumber);
        }

        IfPauseMenuCalled();
        UpdateBestScoreText();
        score = waveNumber;
    }

    private Vector3 GenerateRandomPosition()
    {
        float spawnPosX = Random.Range(-spawnRange,spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPosition = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPosition;
    }

    private void EnemySpawnWave(int number)
    {
        for(int i = 0; i < number; i++)
        {
            Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
        }
    }
    // Check if pause menu called
    // If it is called then stop the game and show the panel
    // Else go back playing
    private void IfPauseMenuCalled()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isMenuActive == false && playerCont.isGameOver == false)
        {
            isMenuActive = true;
            pauseMenu.SetActive(true);
            bestScoreText.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isMenuActive == true)
        {
            isMenuActive = false;
            pauseMenu.SetActive(false);
            bestScoreText.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }
    // Get player information
    void UpdatePlayer()
    {
        playerName = SaveManager.Instance.playerName;
        bestScore = SaveManager.Instance.bestScore;
        Debug.Log("Update Player is Called");
    }
    // Update panel and game scene text
    void UpdateBestScoreText()
    {
        if(bestScore != 0)
        {
            bestScoreText.text = "Best Score : " + playerName + " : " + bestScore;
            bestScoreTextPanel.text = "Best Score : " + playerName + " : " + bestScore;
        }
        else
        {
            bestScoreText.text = "Good Luck with the First Game!";
            bestScoreTextPanel.text = "Good Luck with the First Game!";
        }
    }
    //Chekcs if best score is passed
    private void CheckIfBestScore()
    {
        if(score > bestScore)
        {
            Debug.Log("CheckIfBestScore Inside LOG");
            bestScore = waveNumber;
            SaveManager.Instance.SaveData(waveNumber);
            SaveManager.Instance.LoadData();
        }
        Debug.Log("CheckIfBestScore Outside LOG");
    }
    // Set gameover to true and check if it is high score
    public void GameOver()
    {
        Debug.Log("GameOver");
        playerCont.isGameOver = true;
        bestScoreText.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        CheckIfBestScore();
    }
    // Check if it is high score and restart the game
    public void RestartGame()
    {
        Debug.Log("Restart Game Clicked");
        CheckIfBestScore();
        SceneManager.LoadScene(1);
    }
    // Check if it is high score and go back to main menu
    public void MainMenuButton()
    {
        CheckIfBestScore();
        SceneManager.LoadScene(0);
    }
}

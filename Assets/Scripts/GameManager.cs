using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemyPrefab;
    public GameObject powerUpPrefab;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public TextMeshProUGUI bestScoreTextPanel;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI numberOfStopChange;
    public TextMeshProUGUI numberOfBoostChange;
    public TextMeshProUGUI currentWaveNumber;
    private PlayerController playerCont;

    public string playerName;
    public int highScore;
    
    private float spawnRange = -9;
    private int waveNumber = 1;
    private int numberOfFastEnemy = 1;
    private int numberOfGiantEnemy = 2;
    private bool isMenuClicked;
    private bool isEnemyDown;
    [SerializeField] private int enemyCount;
    [SerializeField] private bool isMenuActive;
    // Start is called before the first frame update
    void Start()
    {
        playerCont = GameObject.Find("Player").GetComponent<PlayerController>();

        EnemySpawnWave(waveNumber);
        Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
        UpdatePlayer();
        UpdateWave();
        UpdateStop();
        UpdateBoost();
    }

    // Update is called once per frame
    void Update()
    {
        WaveCompleted();
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
        // Generate Normal Enemies and Fast Enemies
        if(number % 3 == 0  &&  number % 10 != 0)
        {
            for(int i = 0; i < number - numberOfFastEnemy; i++)
            {
                Instantiate(enemyPrefab[0], GenerateRandomPosition(), enemyPrefab[0].transform.rotation);
            }

            for (int i = 0; i < numberOfFastEnemy; i++)
            {
                Instantiate(enemyPrefab[1], GenerateRandomPosition(), enemyPrefab[1].transform.rotation);
            }
            numberOfFastEnemy++;
        }
        else if(number % 10 != 0)
        {
            for (int i = 0; i < number; i++)
            {
                Instantiate(enemyPrefab[0], GenerateRandomPosition(), enemyPrefab[0].transform.rotation);
            }
        }

        // Generate Giant Enemies with fast enemy
        else if(number % 10 == 0)
        {
            for (int i = 0; i < numberOfGiantEnemy ; i++)
            {
                Instantiate(enemyPrefab[2], GenerateRandomPosition(), enemyPrefab[2].transform.rotation);
                Instantiate(enemyPrefab[1], GenerateRandomPosition(), enemyPrefab[1].transform.rotation);
            }
            numberOfGiantEnemy++;
        }
        
    }

    // Check if pause menu called
    // If it is called then stop the game and show the panel
    // Else go back playing
    private void IfPauseMenuCalled()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isMenuActive == false && playerCont.isGameOver == false)
        {
            isMenuActive = true;
            pauseMenu.SetActive(true);
            bestScoreText.gameObject.SetActive(false);
            Time.timeScale = 0;
        }
        else if ((Input.GetKeyDown(KeyCode.Escape) && isMenuActive == true) || isMenuClicked == true)
        {
            isMenuActive = false;
            isMenuClicked = false;
            pauseMenu.SetActive(false);
            bestScoreText.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }

    // Get player information
    void UpdatePlayer()
    {
        playerName = SaveManager.Instance.playerName;
        highScore = SaveManager.Instance.bestScore;
    }

    // Update panel and game scene text
    void UpdateBestScoreText()
    {
        if(highScore != 0)
        {
            bestScoreText.text = "Best Score : " + playerName + " : " + highScore;
            bestScoreTextPanel.text = "Best Score : " + playerName + " : " + highScore;
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
        if(waveNumber > highScore)
        {
            highScore = waveNumber;
            SaveManager.Instance.SaveData(waveNumber);
            SaveManager.Instance.LoadData();
        }
    }

    //Generate enemies while game is ongoing
    private void IfGameOnGoing()
    {
        enemyCount = GameManager.FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0 && !playerCont.isGameOver)
        {
            waveNumber++;
            Instantiate(powerUpPrefab, GenerateRandomPosition(), powerUpPrefab.transform.rotation);
            EnemySpawnWave(waveNumber);
            UpdateChange();
        }
    }

    // Set gameover to true and check if it is high score
    public void GameOver()
    {
        playerCont.isGameOver = true;
        bestScoreText.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        CheckIfBestScore();
    }

    // Check if it is high score and restart the game
    public void RestartGame()
    {
        if(isMenuActive == true)
        {
            isMenuClicked = true;
        }
        CheckIfBestScore();
        SceneManager.LoadScene(1);
    }

    // Check if it is high score and go back to main menu
    public void MainMenuButton()
    {
        if (isMenuActive == true)
        {
            isMenuClicked = true;
        }
        CheckIfBestScore();
        SceneManager.LoadScene(0);
    }

    public void UpdateStop()
    {
        int numbStop = playerCont.numberOfStopChange;
        numberOfStopChange.text = "#STOP : " + numbStop;
    }

    public void UpdateBoost()
    {
        numberOfBoostChange.text = "#BOOST : " + playerCont.numberOfBoostChange;
    }

    public void UpdateWave()
    {
        currentWaveNumber.text = "Wave Number : " + waveNumber;
    }

    void UpdateChange()
    {
        playerCont.numberOfBoostChange = 5;
        playerCont.numberOfStopChange = 5;
        playerCont.isBoostChangeFinished = false;
        playerCont.isStopChangeFinished = false;
    }

    void WaveCompleted()
    {
        IfGameOnGoing();
        IfPauseMenuCalled();
        UpdateBestScoreText();
        UpdateStop();
        UpdateBoost();
        UpdateWave();
    }
}

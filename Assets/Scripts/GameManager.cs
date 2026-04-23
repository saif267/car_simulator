using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // for restarting scene
using UnityEngine.UI; // for UI elements
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI; // Reference to Game Over panel
    public GameObject mainMenuUI;

    public GameObject difficultyMenuUI;

    private bool isGameOver = false;
    private bool gameStarted = false;

    public Button startButton;
    public Button restartButton;

    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;

    public TMP_Text scoreText;   // TextMeshPro for score

    public TMP_Text gameOverText;
    private int score = 0;

    private float scoreTimer = 0f; // keep a float for smooth increase

    public enum Difficulty { Easy, Medium, Hard }
    public static Difficulty currentDifficulty;


    private void Start()
    {
        Time.timeScale = 0f;

        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(true);
        }


        if (difficultyMenuUI != null)
        { difficultyMenuUI.SetActive(false); }

        // Hook up buttons safely
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(StartGame);
        }
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners();
            startButton.onClick.AddListener(OpenDifficultyMenu);
        }
        gameOverUI.SetActive(false); // hide UI at start

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnRestartPressed);
        }

        if (easyButton != null)
        {
            easyButton.onClick.AddListener(() => SelectDifficulty("Easy"));
        }
        if (mediumButton != null)
        {
            mediumButton.onClick.AddListener(() => SelectDifficulty("Medium"));
        }
        if (hardButton != null)
        {
            hardButton.onClick.AddListener(() => SelectDifficulty("Hard"));
        }

        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }
    }

    private void Update()
    {
        if (gameStarted && !isGameOver)
        {
            scoreTimer += Time.deltaTime * 10f;   // 10 points per second
            score = Mathf.FloorToInt(scoreTimer);
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score;
            }
        }
    }
    public void StartGame()
    {
        Debug.Log("[GameManager] Start Game pressed.");
        gameStarted = true;

        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(false);
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
        }

        // Unfreeze the game
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        // Start delayed UI sequence
        StartCoroutine(GameOverSequence());
    }
    private IEnumerator GameOverSequence()
    {
        // Wait 2 seconds before showing "Game Over" text
        yield return new WaitForSecondsRealtime(2f);

        gameOverUI.SetActive(true);
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
        }

        Debug.Log("[GameManager] Game Over! Final Score = " + score);

        // Wait another 2 seconds before showing Restart button
        yield return new WaitForSecondsRealtime(2f);

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(true);
        }

        // Finally pause the game
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void OpenDifficultyMenu()
    {
        mainMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(true);
    }

    // Step 2: Pick difficulty and start
    public void SelectDifficulty(string difficulty)
    {
        Debug.Log("Difficulty Selected: " + difficulty);
        switch (difficulty)
        {
            case "Easy":
                currentDifficulty = Difficulty.Easy;
                break;
            case "Medium":
                currentDifficulty = Difficulty.Medium;
                break;
            case "Hard":
                currentDifficulty = Difficulty.Hard;
                break;
            default:
                break;
        }
        difficultyMenuUI.SetActive(false);
        StartGame(); // game starts normally for now
    }



    public void RestartGame()
    {
        // This method remains for inspector hookup compatibility
        OnRestartPressed();
    }

    // Actual restart logic, separated so it can be used by both inspector and runtime listeners
    public void OnRestartPressed()
    {
        Debug.Log("[GameManager] Restart button pressed.");
        _ = StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        // Reset timescale in case it was paused
        Time.timeScale = 1f;

        // Small delay to ensure UI feedback registers (optional)
        yield return null;

        // Reload current scene asynchronously (robust)
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!op.isDone)
        {
            yield return null;
        }
    }

}

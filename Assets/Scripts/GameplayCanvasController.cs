using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TimerController))]

public class GameplayCanvasController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [Header("Menus")] 
    [SerializeField] private RectTransform pausedMenu;
    [SerializeField] private RectTransform gameOverMenu;
    [SerializeField] private RectTransform winMenu;


    [Header("Maze renderer")]
    [SerializeField] private GameObject mazeRenderer;

    Scene scene = new Scene();

    private TimerController timerController;
    private int points;
    private int healthPoints;
    private int highScore;



    private void Awake()
    {
        timerController = GetComponent<TimerController>();
    }

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        Hide(pausedMenu);
        Hide(gameOverMenu);
        Hide(winMenu);
        timerController.BeginTimer();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        points = player.getPoints();
        healthPoints = player.getHealthPoints();
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePauseGame();
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ShowGameOverMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Hide(pausedMenu);
        Hide(winMenu);
        Show(gameOverMenu);
        Time.timeScale = 0;
    }    
    public void ShowWinMenu()
    {
        int score = CalculateScore(points, healthPoints);
        string scoreString = $"Your score: {score}";
        scoreText.text = scoreString;
        Hide(pausedMenu);
        Show(winMenu);
        Hide(gameOverMenu);
        Time.timeScale = 0;

        SaveScore();
        loadScores();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        Hide(pausedMenu);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    public void RestartGame()
    {
        Scenes.RestartScene();
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
    public void NextLevel()
    {
        Scenes.LoadNextScene();
        mazeRenderer.gameObject.GetComponent<MazeRenderer>().generatePlayer();
    }
    private void TogglePauseGame()
    {
        if (IsGameOver())
        {
            return;
        }
        if (isGamePaused())
        {
            ResumeGame();
        }
        if (isGameWon()){
            return; 
        }
        else
        {
            PauseGame();
        }
    }


    private void PauseGame()
    {
        Show(pausedMenu);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private bool IsGameOver()
    {
        return gameOverMenu.gameObject.activeInHierarchy;
    }
    private bool isGamePaused()
    {
        return pausedMenu.gameObject.activeInHierarchy;
    }
    private bool isGameWon()
    {
        return winMenu.gameObject.activeInHierarchy;
    }

    private static void Show(Component component)
    {
        component.gameObject.SetActive(true);
    }
    private static void Hide(Component component)
    {
        component.gameObject.SetActive(false);
    }


    private int CalculateScore(int points, int healthPoints)
    {
        int minus = 0;
        double milliseconds = timerController.getTime().TotalMilliseconds;
        if (milliseconds > 10000)
        {
            minus = -20;
        } else if (milliseconds > 15000)
        {
            minus = -50;
        }
        else if (milliseconds > 20000)
        {
            minus = -70;
        }
        else if (milliseconds > 25000)
        {
            minus = -100;
        }
        else if (milliseconds > 30000)
        {
            minus = -200;
        }
        else
        {
            minus = 0;
        }

        int sc = points * 100 + healthPoints * 50 + minus;

        return sc;
    }

    private void SaveScore()
    {
        int highScore = PlayerPrefs.GetInt($"Score{scene.name}");
        if (highScore == 0)
        {
            PlayerPrefs.SetInt($"Score{scene.name}", CalculateScore(points, healthPoints));
        }
        else if (highScore < CalculateScore(points, healthPoints))
        {
            PlayerPrefs.SetInt($"Score{scene.name}", CalculateScore(points, healthPoints));
        }
        
    }
    public void loadScores()
    {
        highScore = PlayerPrefs.GetInt($"Score{scene.name}");
        highScoreText.text = $"Your highscore = {highScore}";
    }
}

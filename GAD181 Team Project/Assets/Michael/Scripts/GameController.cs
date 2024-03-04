using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("References")]
    public RunnerPlayerControls playerControls;
    public GameObject GameOverScreen;
    public GameObject EndGameScreen;
    public GameObject StartGameScreen;
    public GameObject GameView;
    public HurdleSpawner spawner;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    [Header("Player")]
    public bool gameRunning = false;
    public bool stopScrolling = false;
    public float ScrollSpeed = -1.5f;

    [Header("Time")]
    public float timeInGame;
    public float TimeRemaining;

    public float startingTime = 10f;

    [Header("Score")]
    public int Score;

    public void Start()
    {
        Score = 0;
        timeInGame = 0f;
        TimeRemaining = startingTime;
    }

    public void Update()
    {
        timerText.text = TimeRemaining.ToString();
        timeInGame += Time.deltaTime;
        if (gameRunning)
        {
            if (TimeRemaining >= 0)
            {
                TimeRemaining -= Time.deltaTime;
            }
            else
            {
                EndGame();
            }
        }
    }

    public void GameOver()
    {
        playerControls.enableControls = false;
        gameRunning = false;
        stopScrolling = true;
        spawner.gameRunning = false;
        GameOverScreen.SetActive(true);
        GameView.SetActive(false);
    }

    public void StartGame()
    {
        spawner.IntHurdles();
        playerControls.enableControls = true;
        gameRunning = true;
        stopScrolling = false;
        spawner.gameRunning = true;
        StartGameScreen.SetActive(false);
        GameView.SetActive(true);
        
        StartScrolling();
    }

    public void EndGame()
    {
        playerControls.enableControls = false;
        stopScrolling = true;
        gameRunning = false;
        spawner.gameRunning = false;
        EndGameScreen.SetActive(true);
        GameView.SetActive(false);
    }

    public void AddScore(int score)
    {
        if(!gameRunning)
        {
            return;
        }
        Score += score;
        scoreText.text = Score.ToString();

    }

    public void StartScrolling()
    {
        scrollingObject[] scroll = new scrollingObject[5];
        for(int i = 0; i < 5; i++)
        {
            scrollingObject obj = FindObjectOfType<scrollingObject>();
            if(obj != null)
            {
                scroll[i] = obj;
                scroll[i].StartScroll();
            }
            else
            {
                Debug.Log("no obj in scene");
            }
        }
    }
}

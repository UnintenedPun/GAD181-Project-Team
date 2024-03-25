using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapFroogManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject StartGameUI;
    public GameObject GameOverUI;
    public GameObject WonGameUI;
    public GameObject InGameUI;

    [Header("Variablies")]
    public bool gameRunning = false;
    public int gameScore;
    public float timeBetweenspawns;
    public GameObject BirdPrefab;

    [Header("private Variables")]
    private int currentScore;
    private bool spawningBirdsX = false;
    private bool spawningBirdsY = false;
    private BirdController birdController;

    private void Start()
    {
        birdController = FindObjectOfType<BirdController>();
    }



    #region Functions
    /// <summary>
    /// Starts the game and controls the UI and other managers/Player controlss
    /// </summary>
    public void StartGame()
    {
        gameRunning = true;
        StartGameUI.SetActive(false);
        InGameUI.SetActive(true);
        StartSpawningBirds();
    }

    /// <summary>
    /// Controlls the Win condition of the game, and gets called when the game has been WON
    /// </summary>
    public void WinCondition()
    {
        gameRunning = false;
        InGameUI.SetActive(false);
        WonGameUI.SetActive(true);
    }

    /// <summary>
    /// Controlls the lose condition of the game, and gets called when the game has been LOST
    /// </summary>
    public void LoseCondition() 
    {
        gameRunning = false;
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        gameScore = currentScore;
    }

    private void StartSpawningBirds()
    {
        if(!spawningBirdsX)
        {
            StartCoroutine(SpawnBirdsX());
        }
        
        if(!spawningBirdsY)
        {
            StartCoroutine(SpawnBirdsY());
        }
        
    }

    private IEnumerator SpawnBirdsX()
    {
        spawningBirdsX = true;
        
       yield return new WaitForSeconds(timeBetweenspawns);

        birdController.SpawnBirdX(BirdPrefab);
        spawningBirdsX = false;
    }

    private IEnumerator SpawnBirdsY()
    {
        spawningBirdsY = true;

        yield return new WaitForSeconds(timeBetweenspawns);

        birdController.SpawnBirdy(BirdPrefab);
        spawningBirdsY = false;
    }

    #endregion
}

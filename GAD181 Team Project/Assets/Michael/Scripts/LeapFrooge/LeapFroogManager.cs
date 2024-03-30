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

    [Header("Birds")]
    private bool spawningBirdsX = false;
    private bool spawningBirdsY = false;
    public GameObject BirdPrefabLeft;
    public GameObject BirdPrefabRight;
    private BirdController birdController;
    public int maxBirdCount;

    [Header("private Variables")]
    private int currentScore;


    [HideInInspector] public int birdCount;

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
        if(!spawningBirdsX && birdCount < maxBirdCount)
        {
            StartCoroutine(SpawnBirdsX());
        }
        
        if(!spawningBirdsY && birdCount < maxBirdCount)
        {
            StartCoroutine(SpawnBirdsY());
        }
        
    }

    private IEnumerator SpawnBirdsX()
    {
        spawningBirdsX = true;
        
       yield return new WaitForSeconds(timeBetweenspawns);

        birdController.SpawnBirdX(BirdPrefabLeft);
        birdCount++;
        spawningBirdsX = false;
        StartSpawningBirds();
    }

    private IEnumerator SpawnBirdsY()
    {
        spawningBirdsY = true;

        yield return new WaitForSeconds(timeBetweenspawns);

        birdController.SpawnBirdy(BirdPrefabRight);
        birdCount++;
        spawningBirdsY = false;
        StartSpawningBirds();
    }

    #endregion
}

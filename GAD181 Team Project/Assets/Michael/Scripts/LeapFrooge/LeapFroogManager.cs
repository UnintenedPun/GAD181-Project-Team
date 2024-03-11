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


    #region Functions
    /// <summary>
    /// Starts the game and controls the UI and other managers/Player controlss
    /// </summary>
    public void StartGame()
    {
        StartGameUI.SetActive(false);
        InGameUI.SetActive(true);
    }

    /// <summary>
    /// Controlls the Win condition of the game, and gets called when the game has been WON
    /// </summary>
    public void WinCondition()
    {
        InGameUI.SetActive(false);
        WonGameUI.SetActive(true);
    }

    /// <summary>
    /// Controlls the lose condition of the game, and gets called when the game has been LOST
    /// </summary>
    public void LoseCondition() 
    {
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }


    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurdle : MonoBehaviour
{
    public GameController Controller;
    public int ScoreAmount;

    public void Start()
    {
        Controller = FindAnyObjectByType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<RunnerPlayerControls>() != null)
        {
            Controller.AddScore(ScoreAmount);
        }
    }

}

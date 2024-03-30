using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int scoreAmount;

    private LeapFroogManager manager;

    private void Start()
    {
        manager = FindFirstObjectByType<LeapFroogManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            manager.AddScore(scoreAmount);
            this.gameObject.SetActive(false);
        }

    }
}

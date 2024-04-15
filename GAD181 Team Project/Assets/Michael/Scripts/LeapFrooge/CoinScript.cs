using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public int scoreAmount;

    private Animator animator;
    private LeapFroogManager manager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        manager = FindFirstObjectByType<LeapFroogManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetTrigger("Pickup");
        }

    }

    public void AddScore()
    {
        manager.AddScore(scoreAmount);
        this.gameObject.SetActive(false);
    }
}

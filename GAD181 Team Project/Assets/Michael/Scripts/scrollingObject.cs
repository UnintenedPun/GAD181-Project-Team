using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingObject : MonoBehaviour
{
    public GameController controller;
    public Rigidbody2D rb2d;

    public void Start()
    {
        controller = FindObjectOfType<GameController>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.gameRunning == false)
        {
            rb2d.velocity = Vector2.zero;
        }
        else
        {
            StartScroll();
        }
    }

    public void StartScroll()
    {
        if(rb2d == null)
        {
            controller = FindObjectOfType<GameController>();
            rb2d = GetComponent<Rigidbody2D>();
        }
        rb2d.velocity = new Vector2(controller.ScrollSpeed, 0);
    }
}

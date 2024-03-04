using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerPlayerControls : MonoBehaviour
{
    [Header("Private Variables")]
    private Rigidbody2D rb;
    private GameController controller;

    [Header("Public Variables")]
    public float jumpForce;
    public LayerMask ground;
    public bool enableControls = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = FindAnyObjectByType<GameController>();
    }


    public void Update()
    {
        if(enableControls)
        {
            if (rb != null)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.velocity = new Vector2(0, jumpForce);
                    Debug.Log("Jump");
                }
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hurdle"))
        {
            controller.GameOver();
        }
    }
}

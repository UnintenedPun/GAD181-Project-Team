using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerPlayerControls : MonoBehaviour
{
    [Header("Private Variables")]
    private Rigidbody2D rb;
    private GameController controller;
    private Collider2D col;

    [Header("Public Variables")]
    public Animator animator;
    public float jumpForce;
    public LayerMask ground;
    public bool enableControls = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = FindAnyObjectByType<GameController>();
        col = GetComponent<Collider2D>();
    }


    public void Update()
    {
        if(enableControls)
        {
            if (rb != null)
            {
                if (Input.GetKeyDown(KeyCode.Space) && col.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(0, jumpForce);
                    
                    Debug.Log("Jump");
                }
                animator.SetFloat("JumpVelocity", rb.velocity.y);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hurdle"))
        {
            animator.SetTrigger("Damaged");
            
        }
    }
}

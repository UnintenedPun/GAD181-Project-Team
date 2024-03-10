using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerPlayerControls : MonoBehaviour
{
    [Header("Private Variables")]
    private Rigidbody2D rb;
    private GameController controller;
    private Collider2D col;
    private int currentHealth;
    private bool waitTime = false;

    [Header("Public Variables")]
    public int health;
    public Animator animator;
    public float jumpForce;
    public LayerMask ground;
    public bool enableControls = false;

    public void Start()
    {
        currentHealth = health;
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
            if(!waitTime)
            StartCoroutine(Damaged());
        }
    }

    private void AddHealth(int amount)
    {
        if(currentHealth <= health && currentHealth > 0)
        {
            currentHealth += amount;
            Debug.Log("health at" + currentHealth);
        }
        else
        {
            controller.GameOver();
        }
    }

    private IEnumerator Damaged()
    {
        waitTime = true;
        controller.ScrollSpeed = -1f;
        animator.SetTrigger("Damaged");
        yield return new WaitForSeconds(1.5f);
        controller.ScrollSpeed = -3f;
        AddHealth(-1);
        waitTime = false;
    }
}

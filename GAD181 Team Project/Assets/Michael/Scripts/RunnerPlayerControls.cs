using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerPlayerControls : MonoBehaviour
{
    [Header("Private Variables")]
    private Rigidbody2D rb;

    [Header("Public Variables")]
    public float jumpForce;
    public LayerMask ground;
    public bool enableControls = true;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
                }
            }
        }
    }
}

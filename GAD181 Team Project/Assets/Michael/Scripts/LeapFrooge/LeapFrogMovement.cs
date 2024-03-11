using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapFrogMovement : MonoBehaviour
{
    [Header("Variables")]
    private Vector2 Yaxis;
    private Vector2 Xaxis;

    [Header("References")]
    private Rigidbody2D rb2d;



    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Yaxis = new Vector2 (0, 1);
        Xaxis = new Vector2 (1, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            rb2d.MovePosition(rb2d.position - Xaxis);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            rb2d.MovePosition(rb2d.position + Xaxis);
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            rb2d.MovePosition(rb2d.position + Yaxis);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public bool moveleft;
    public bool moveright;
    public bool isActive;
    public bool Move;
    public float speed;

    private Vector2 Xaxis;
    private Rigidbody2D rb2d;


    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Xaxis = new Vector2(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Move)
        {
            if(moveleft)
            {
                rb2d.MovePosition(rb2d.position - Xaxis * speed);
            }
            else if(moveright)
            {
                rb2d.MovePosition(rb2d.position + Xaxis * speed);
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            LeapFrogMovement sub =  collision.gameObject.GetComponent<LeapFrogMovement>();
            if(sub != null)
            {
                if(sub.hitTimes > 0)
                {
                    sub.hitTimes--;
                }
                else
                {
                    sub.birdThatHasCapturedMe = this.gameObject;
                    sub.isCaptured = true;
                }
                
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            LeapFrogMovement sub = collision.gameObject.GetComponent<LeapFrogMovement>();
            if (sub != null)
            {
                sub.ResetVelocity();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.FilePathAttribute;

public class LeapFrogMovement : MonoBehaviour
{
    [Header("Variables")]
    private Vector2 Yaxis;
    private Vector2 Xaxis;

    [Header("References")]
    private Rigidbody2D rb2d;
    private Transform myPos;
    private LeapFroogManager controller;
    private bool controllsEnabled = true;
    private Animator myAnimator;
    private bool wallinFront1 = false;
    private bool wallinFront2 = false;


    public int hitTimes = 3;
    public Transform GroundCheck;
    public Tilemap deadZone;
    public Tilemap background;
    public Tilemap PlayBarrier;
    public Tilemap WinCondition;
    public Tilemap walls1;
    public Tilemap walls2;

    public bool isCaptured;
    public GameObject birdThatHasCapturedMe;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myPos = GetComponent<Transform>();
        controller = FindFirstObjectByType<LeapFroogManager>();
        rb2d = GetComponent<Rigidbody2D>();
        Yaxis = new Vector2 (0, 1);
        Xaxis = new Vector2 (1, 0);
        
    }

    // Update is called once per frame
    void Update()
    {

        if(IsWallInFront1())
        {
            wallinFront1 = true;
        }
        else
        {
            wallinFront1 = false;
        }

        if (IsWallInFront2())
        {
            wallinFront2 = true;
        }
        else
        {
            wallinFront2 = false;
        }

        if (birdThatHasCapturedMe != null)
        {
            myPos.position = birdThatHasCapturedMe.transform.position;
            if(this.gameObject.GetComponent<Collider2D>().enabled != false)
            {
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                myAnimator.SetBool("isCaptured", true);
            }
            
            CheckCurrentPos();
            return;
        }


        if(controller.gameRunning && controllsEnabled && !isCaptured)
        {
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                CheckCondition();
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                CheckCondition();
                MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                CheckCondition();
                MoveUp();
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                CheckCondition();
                MoveDown();
            }
        }
        
    }


    #region Funtions

    private void CheckCondition()
    {
        CheckCurrentPos();
        CheckPlayBarrier();
        CheckWinPos();
    }

    private void MoveLeft()
    {
        myAnimator.SetTrigger("IsMoving");
        rb2d.MovePosition(rb2d.position - Xaxis);
    }

    private void MoveRight()
    {
        myAnimator.SetTrigger("IsMoving");
        rb2d.MovePosition(rb2d.position + Xaxis);
    }

    private void MoveUp()
    {
        if (wallinFront1 == true)
        {
            return;
        }

        if (wallinFront2 == true)
        {
            return;
        }
        myAnimator.SetTrigger("IsMoving");
        rb2d.MovePosition(rb2d.position + Yaxis);
    }

    private void MoveDown()
    {
        if (wallinFront1 == true)
        {
            return;
        }

        if (wallinFront2 == true)
        {
            return;
        }
        myAnimator.SetTrigger("IsMoving");
        rb2d.MovePosition(rb2d.position - Yaxis);
    }

    public void ResetVelocity()
    {
        rb2d.velocity = Vector3.zero;
        Vector3Int pos = background.LocalToCell(myPos.position);

        myPos.position = background.LocalToCell(pos);
    }

    private void CheckCurrentPos()
    {
        if(TileCheck(myPos, deadZone))
        {
            controller.LoseCondition();
        }
    }

    private void CheckWinPos()
    {
        if(TileCheck(myPos, WinCondition))
        {
            controller.WinCondition();
        }
 
    }

    private void CheckPlayBarrier()
    {
        if(TileCheck(GroundCheck, PlayBarrier))
        {
            controllsEnabled = false;
        }
        else if(TileCheck(myPos, PlayBarrier))
        {
            controllsEnabled = false;
        }
    }

    private bool IsWallInFront2()
    {
        Debug.Log("checking walls 2");
        Debug.Log(TileCheck(GroundCheck, walls2));
        return TileCheck(GroundCheck, walls2);
    }

    private bool IsWallInFront1()
    {
        Debug.Log("checking walls 1");
        Debug.Log(TileCheck(GroundCheck, walls1));
        return TileCheck(GroundCheck, walls1);
    }

    private bool TileCheck(Transform pos, Tilemap checkMap)
    {
        Debug.Log("Checking pos" +  pos + "and Tile Map" + checkMap);
        Vector3Int newPos = checkMap.LocalToCell(pos.position);
        TileBase location = checkMap.GetTile(newPos);

        Debug.Log(location);

        if(checkMap.isActiveAndEnabled == false)
        {
            Debug.Log("nulling");
            location = null;
        }

        if (location != null)
        {
            Debug.Log("true");
            return true;
        }
        else
        {
            Debug.Log("false");
            return false;
        }
    }
    #endregion
}

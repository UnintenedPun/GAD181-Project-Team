using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeapFrogMovement : MonoBehaviour
{
    [Header("Variables")]
    private Vector2 Yaxis;
    private Vector2 Xaxis;

    [Header("References")]
    private Rigidbody2D rb2d;
    private Transform myPos;
    private LeapFroogManager controller;

    public Transform GroundCheck;
    public Tilemap deadZone;
    public Tilemap walls1;
    public Tilemap walls2;


    // Start is called before the first frame update
    void Start()
    {
        myPos = GetComponent<Transform>();
        controller = FindFirstObjectByType<LeapFroogManager>();
        rb2d = GetComponent<Rigidbody2D>();
        Yaxis = new Vector2 (0, 1);
        Xaxis = new Vector2 (1, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.gameRunning)
        {
            CheckCurrentPos();
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                MoveUp();
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                MoveDown();
            }
        }
        
    }


    #region Funtions

    private void MoveLeft()
    {
        rb2d.MovePosition(rb2d.position - Xaxis);
    }

    private void MoveRight()
    {
        rb2d.MovePosition(rb2d.position + Xaxis);
    }

    private void MoveUp()
    {
        if (IsWallInFront() == true)
        {
            return;
        }
        rb2d.MovePosition(rb2d.position + Yaxis);
    }

    private void MoveDown()
    {
        rb2d.MovePosition(rb2d.position - Yaxis);
    }

    private void CheckCurrentPos()
    {
        Vector3Int pos = deadZone.LocalToCell(myPos.position);
        TileBase location = deadZone.GetTile(pos);
        if (location != null)
        {
            controller.LoseCondition();
        }
        else
        {
            return;
        }
    }

    private bool IsWallInFront()
    {
        Vector3Int pos1 = walls1.LocalToCell(GroundCheck.position);
        Vector3Int pos2 = walls2.LocalToCell(GroundCheck.position);

        TileBase location1 = walls1.GetTile(pos1);
        TileBase location2 = walls2.GetTile(pos2);

        Debug.Log(location1);
        Debug.Log(location2);
        if(walls1.isActiveAndEnabled == false)
        {
            location1 = null;
        }
        if(walls2.isActiveAndEnabled == false)
        {
            location2 = null;
        }
        if(location1 != null || location2 != null)
        {
            return true;
        }
        else
        { 
            return false; 
        }
    }

    #endregion
}

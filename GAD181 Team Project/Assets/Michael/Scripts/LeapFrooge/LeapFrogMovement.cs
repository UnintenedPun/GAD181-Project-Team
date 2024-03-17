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

    public Tilemap deadZone;


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
            CheckPos();
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
        rb2d.MovePosition(rb2d.position + Yaxis);
    }

    private void CheckPos()
    {
        Vector3Int pos = deadZone.LocalToCell(myPos.position);
        TileBase location = deadZone.GetTile(pos);
        Debug.Log(location);
        if (location != null)
        {
            controller.LoseCondition();
        }
        else
        {
            return;
        }
    }

    #endregion
}

using UnityEngine;
using UnityEngine.Tilemaps;

public class LeapFrogMovement : MonoBehaviour
{
    [Header("Variables")]
    private Vector2 Yaxis;
    private Vector2 Xaxis;

    [Header("References")]
    public Rigidbody2D rb2d;
    private Transform myPos;
    private LeapFroogManager controller;
    public bool controllsEnabled = true;
    public bool moveForward = true;
    private Animator myAnimator;


    public int hitTimes = 3;
    public Transform GroundCheck;
    public Tilemap[] Walls;
    public Tilemap deadZone;
    public Tilemap background;
    public Tilemap PlayBarrier;
    public Tilemap WinCondition;

    public bool isCaptured;
    public GameObject birdThatHasCapturedMe;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myPos = GetComponent<Transform>();
        controller = FindFirstObjectByType<LeapFroogManager>();
        Yaxis = new Vector2(0, 1);
        Xaxis = new Vector2(1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.gameRunning)
        {
            CheckMyPos();
            CheckGroundCheck();

            if (birdThatHasCapturedMe != null && isCaptured)
            {
                myPos.position = birdThatHasCapturedMe.transform.position;
                if (this.gameObject.GetComponent<Collider2D>().enabled != false)
                {
                    this.gameObject.GetComponent<Collider2D>().enabled = false;
                    myAnimator.SetBool("isCaptured", true);
                }

                return;
            }

            if(controllsEnabled)
            {
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
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    MoveDown();
                }
            }
            
        }

    }


    #region Funtions

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
        if (moveForward)
        {
            myAnimator.SetTrigger("IsMoving");
            rb2d.MovePosition(rb2d.position + Yaxis);
        }
        else
        {
            myAnimator.SetTrigger("Stagger");
        }
    }

    private void MoveDown()
    {
        myAnimator.SetTrigger("IsMoving");
        rb2d.MovePosition(rb2d.position - Yaxis);
    }

    private bool TileCheck(Transform pos, Tilemap checkMap)
    {
        
        Vector3Int newPos = checkMap.LocalToCell(pos.position);
        TileBase location = checkMap.GetTile(newPos);

        if (checkMap.isActiveAndEnabled == false)
        {

            location = null;
        }

        if (location != null)
        {

            return true;
        }
        else
        {

            return false;
        }
    }

    private void CheckMyPos()
    {
        if (TileCheck(myPos, deadZone))
        {
            controller.LoseCondition();
        }

        if(TileCheck(myPos, WinCondition)) 
        {
            controller.WinCondition();
        }
    }

    private void CheckGroundCheck()
    {
        if (TileCheck(GroundCheck, Walls[1]) || TileCheck(GroundCheck, Walls[0]) || TileCheck(GroundCheck, PlayBarrier))
        {
            moveForward = false;
        }
        else
        {
            moveForward = true;
        }
    }
    #endregion
}

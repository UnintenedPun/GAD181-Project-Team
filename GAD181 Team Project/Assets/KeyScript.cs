using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyScript : MonoBehaviour
{
    public GameObject doors;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider != null && collision.collider.tag == "Player")
        {
            PickUp();
        }
        
    }

    private void PickUp()
    {
        doors.SetActive(false);
        this.gameObject.SetActive(false);
    }
}

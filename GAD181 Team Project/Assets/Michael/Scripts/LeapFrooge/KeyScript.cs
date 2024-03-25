using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyScript : MonoBehaviour
{
    public GameObject doors;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
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

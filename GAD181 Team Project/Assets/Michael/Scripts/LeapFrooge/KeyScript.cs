using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KeyScript : MonoBehaviour
{
    public GameObject doors;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.tag == "Player")
        {
            animator.SetTrigger("Pickup");
        }
    }

    private void PickUp()
    {
        doors.SetActive(false);
        this.gameObject.SetActive(false);
    }
}

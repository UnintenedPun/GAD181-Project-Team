using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdRemoveScript : MonoBehaviour
{
    private LeapFroogManager manager;

    private void Start()
    {
        manager = FindFirstObjectByType<LeapFroogManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bird")
        {
            BirdScript birdScript = collision.GetComponent<BirdScript>();
            if(birdScript != null)
            {
                birdScript.Move = false;
                birdScript.isActive = false;
                manager.birdCount--;
                Destroy(birdScript.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public Transform[] Spawnpointsx;
    public Transform[] Spawnpointsy;

    public void SpawnBirdX(GameObject birdPrefab)
    {
        BirdScript script = birdPrefab.GetComponent<BirdScript>();
      
        if (birdPrefab != null && !script.isActive)
        {
            Debug.Log("Spawning birdX");
            int randomvalx = Random.Range(0, Spawnpointsx.Length);
            GameObject bird = Instantiate(birdPrefab, Spawnpointsx[randomvalx].position, Quaternion.identity);
            BirdScript currentBird = bird.GetComponent<BirdScript>();
            currentBird.Move = true;
            currentBird.isActive = true;
        }
    }

    public void SpawnBirdy(GameObject birdPrefab)
    {
        BirdScript script = birdPrefab.GetComponent<BirdScript>();

        if (birdPrefab != null && !script.isActive)
        {
            Debug.Log("Spawning birdY");
            int randomvaly = Random.Range(0, Spawnpointsy.Length);
            GameObject bird = Instantiate(birdPrefab, Spawnpointsy[randomvaly].position, Quaternion.identity);
            BirdScript currentBird = bird.GetComponent<BirdScript>();
            currentBird.Move = true;
            currentBird.isActive = true;
        }
    }
}

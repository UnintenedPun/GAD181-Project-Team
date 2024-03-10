using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleSpawner : MonoBehaviour
{
    public int HurdlePoolSize = 3;
    public GameObject[] HurdlePrefab;
    public GameObject spawnpoint;
    public float spawnRate = 4f;

    private GameObject[] Hurdles;
    private float timeSinceLastSpawn;
    private int currentHurdle = 0;

    public bool gameRunning;

    public void Update()
    {
        if (gameRunning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnRate)
            {
                timeSinceLastSpawn = 0;
                Hurdles[currentHurdle].transform.position = spawnpoint.transform.position;
                currentHurdle++;

                if (currentHurdle >= HurdlePoolSize)
                {
                    currentHurdle = 0;
                }

            }
        }
    }

    public void IntHurdles()
    {
        Hurdles = new GameObject[HurdlePoolSize];
        for (int i = 0; i < HurdlePoolSize; ++i)
        {
            Hurdles[i] = (GameObject)Instantiate(HurdlePrefab[Random.Range(0, HurdlePoolSize)], spawnpoint.transform.position, Quaternion.identity);
        }
    }
}

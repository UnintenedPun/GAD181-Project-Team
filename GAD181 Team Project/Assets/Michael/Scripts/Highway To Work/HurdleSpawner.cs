using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurdleSpawner : MonoBehaviour
{
    public int HurdlePoolSize;
    public GameObject[] HurdlePrefab;
    public GameObject spawnpoint;
    public float spawnRate;

    private GameObject[] Hurdles;
    private float timeSinceLastSpawn;
    private int HurdlesInGame;
    private int currentHurdle = 0;

    public bool gameRunning;

    public void Start()
    {
        Hurdles = new GameObject[HurdlePoolSize];
        SpawnHurdle(1);
    }


    public void Update()
    {
        if (gameRunning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= spawnRate)
            {
                timeSinceLastSpawn = 0;
                if (Hurdles[currentHurdle] == null)
                {
                    SpawnHurdle(1);
                    if (currentHurdle >= HurdlePoolSize)
                    {
                        currentHurdle = 0;
                    }
                    return;
                }
                MoveHurdle();

                if (currentHurdle >= HurdlePoolSize)
                {
                    currentHurdle = 0;
                }
            }
        }
    }

    public void SpawnHurdle(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Hurdles[i] = (GameObject)Instantiate(HurdlePrefab[Random.Range(0, HurdlePoolSize)], spawnpoint.transform.position, Quaternion.identity);
        }
        MoveHurdle();
    }

    private void MoveHurdle()
    {
        if(Hurdles[currentHurdle] != null)
        {
            Hurdles[currentHurdle].transform.position = spawnpoint.transform.position;
            currentHurdle++;
        }
            
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{

    [SerializeField] GameObject sphere;
    [SerializeField] int spawnTime;
    [SerializeField] int spawnDelay;

    bool stopSpawning = false;
    
    void Start()
    {
        InvokeRepeating("SpawnSphere", spawnTime, spawnDelay);
    }

    public void SpawnSphere()
    {
        Instantiate(sphere, transform.position, transform.rotation);
        if (stopSpawning)
        {
            CancelInvoke("SpawnSphere");
        }
    }
}

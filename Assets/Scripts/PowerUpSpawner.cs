using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUp;
    private float spawnRate = 10f;
    private float _nextSpawn = 2f;

    void Update()
    {
        if(Time.time > _nextSpawn){
            // Respawn a powerUp in a random position
            Instantiate(powerUp, new Vector3(Random.Range(-8,8), 7, 0), Quaternion.Euler(0,0,-32));
            // Set next spawn time
            _nextSpawn = Time.time + spawnRate;
        }

    }
}

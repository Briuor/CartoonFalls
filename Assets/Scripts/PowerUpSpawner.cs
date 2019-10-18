using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUp;
    private float spawnRate = 10f;
    private float _nextSpawn = 2f;
    private float posX = 0f;
    private float posY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > _nextSpawn){
            // Respawn a powerUp in a random position
            Instantiate(powerUp, new Vector3(Random.Range(-8,8), 7, 0), Quaternion.identity);
            // Set next spawn time
            _nextSpawn = Time.time + spawnRate;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject powerUp;
    public float spawnRate = 30f;
    private float _nextSpawn = 0f;
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
            Instantiate(powerUp, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            powerUp.GetComponent<PowerUp>().setRandomPowerUpType();

            // Set next spawn time
            _nextSpawn = Time.time + spawnRate;
        }

    }
}

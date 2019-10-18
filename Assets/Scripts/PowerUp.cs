using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType : int
{
    TRIPLE_JUMP,
    SUPER_PUNCH,
    ULTRA_RIGID,
    ULTRA_SPEED,
    ULTRA_SLOW,
    FEATHER_WEIGHT,
    BAD_LUCKY

}

public class PowerUp : MonoBehaviour {
    private float duration = 10f;
    private float lifetime = 15f;
    public GameObject pickupEffect;
    public AudioClip powerUpClip;
    public AudioSource audioSource;
    public PowerUpType powerUpID;

    void Start()
    {
        audioSource.PlayOneShot(powerUpClip);
        powerUpID = (PowerUpType) Random.Range(0, System.Enum.GetValues(typeof(PowerUpType)).Length);
        Debug.Log(powerUpID);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {   
        if(other.CompareTag("Player"))
        {
            Pickup(other);
        } 
        else if(other.CompareTag("Finish"))
        {
                Destroy(this.gameObject);
        }
        else 
        {
             Destroy(this.gameObject, lifetime);
        } 
    }

    private void Pickup(Collider2D playerDetected)
    {   
        // Catching the player
        PlayerController player = playerDetected.GetComponent<PlayerController>();

        // Visual pickup effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        
        audioSource.PlayOneShot(powerUpClip);
        
        player.PowerUpOn(this.powerUpID,this.duration);

        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType : int
{
    FEATHER_WEIGHT,
    TRIPLE_JUMP,
    ULTRA_RIGID,
    ULTRA_SPEED,
    SUPER_PUNCH,
    ULTRA_SLOW,
    CONFUSED,
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
        // audioSource.PlayOneShot(powerUpClip);
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
        PlayerControl player = playerDetected.GetComponent<PlayerControl>();

        // Visual pickup effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        
        audioSource.clip = powerUpClip;
        audioSource.Play();
        
        player.PowerUpOn(this.powerUpID,this.duration);

        Destroy(this.gameObject, audioSource.clip.length);
    }
}

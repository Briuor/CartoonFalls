using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    public float multiplier = 1.4f;
    public float duration = 4f;
    public GameObject pickupEffect;
    public AudioClip powerUpClip;
    public AudioSource audioSource;
    public PowerUpType powerUpID;

    public void setRandomPowerUpType() {
        powerUpID = (PowerUpType) Random.Range(0, System.Enum.GetValues(typeof(PowerUpType)).Length);
        Debug.Log(powerUpID);
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine( Pickup(other) );
        }    
    }

    IEnumerator Pickup(Collider2D player)
    {
        Debug.Log("Colidiu!");
        PlayerController stats = player.GetComponent<PlayerController>();
        // Apply powerUp effect
        switch(powerUpID)
        {
            /*case PowerUpType.TRIPLE_JUMP:
            break;
            
            case PowerUpType.SUPER_PUNCH:
            break;

            case PowerUpType.ULTRA_RIGID:
            break;

            case PowerUpType.ULTRA_SLOW:
            break;*/

            case PowerUpType.ULTRA_SPEED:
                stats.speed *= multiplier;
                yield return new WaitForSeconds(duration);
                stats.speed /= multiplier;
            break;

            //case PowerUpType.FEATHER_WEIGHT:
            //break;

            case PowerUpType.BAD_LUCKY:
                stats.jumpForce /= multiplier;
                yield return new WaitForSeconds(duration);
                stats.jumpForce *= multiplier;
            break;
        }

        // Visual pickup effect
        Instantiate(pickupEffect, transform.position, transform.rotation);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(powerUpClip);
        }

        Destroy(this.gameObject);
    }

    public enum PowerUpType : int
    {
        ULTRA_SPEED,
        BAD_LUCKY

    }
}

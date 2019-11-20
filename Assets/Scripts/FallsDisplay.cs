using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallsDisplay : MonoBehaviour
{
    PlayerControl playerStats;
    public Text fallText;
    void Start()
    {   
        GameObject player;
        
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerControl>(); 
    }
    void Update()
    {
        fallText.text = playerStats.numberOfFalls + " FALLS";
    }
}

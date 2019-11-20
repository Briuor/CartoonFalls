using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    PlayerControl playerStats;
    PlayerControl player2Stats;
    public Text fallText1;
    public Text fallText2;
    public Text timeText;


    public void UpdateChronometer(int time)
    {
        timeText.text = time.ToString();
    }


    void Start()
    {
        GameObject player;
        
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerControl>();

        player = GameObject.Find("Player 2");
        player2Stats = player.GetComponent<PlayerControl>(); 
    }

    // Update is called once per frame
    void Update()
    {
      fallText1.text = playerStats.numberOfFalls + " FALLS";
      fallText2.text = player2Stats.numberOfFalls + " FALLS";  
    }
}

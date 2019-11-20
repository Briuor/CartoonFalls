using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public GameObject main;
    static public float fightTime;
    public GameObject HUD;
    HUDControl gameController;
    AudioSource mainAudio;
    // Start is called before the first frame update
    void Start()
    {
       mainAudio =  main.GetComponent<AudioSource>();
       fightTime = 120f;
       gameController = HUD.GetComponent<HUDControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(PauseMenu.gameIsPaused)
        {
            mainAudio.Pause();
        } else {
            mainAudio.UnPause();
        }
        
        fightTime -= Time.deltaTime;
        gameController.UpdateChronometer((int) Mathf.Round(fightTime));
        
    }
}

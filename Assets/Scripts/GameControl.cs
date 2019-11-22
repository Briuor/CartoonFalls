using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public GameObject main;
    static public float fightTime;
    public GameObject HUD;
    HUDControl gameController;
    // Start is called before the first frame update
    
    void Start()
    {   
        Time.timeScale = 1f;
        fightTime = 120f;
        gameController = HUD.GetComponent<HUDControl>();
    }

    // Update is called once per frame
    void Update()
    {      
        fightTime -= Time.deltaTime;
        //Debug.Log("Pausado: " + PauseMenu.gameIsPaused + " - Finalizado:" + FinalScreenMenu.gameIsFinished + " - Time: " + Time.timeScale);
        if(fightTime <= 0)
            gameController.ChooseWinner();
        else { 
            gameController.UpdateChronometer((int) Mathf.Round(fightTime));
        }
        
    }
}

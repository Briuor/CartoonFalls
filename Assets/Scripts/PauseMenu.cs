using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;


    void Update()
    {      
        if(Keyboard.current.spaceKey.wasReleasedThisFrame 
            || ( Gamepad.current != null 
            &&   Gamepad.current.startButton.wasReleasedThisFrame))
        {

            gameIsPaused = !gameIsPaused;
            pauseMenuUI.SetActive(gameIsPaused);
            Time.timeScale = !gameIsPaused ? 1f : 0f;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu_Personagem");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}

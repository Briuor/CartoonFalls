using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InitialMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasReleasedThisFrame 
            || ( Gamepad.current != null 
            &&   Gamepad.current.startButton.wasReleasedThisFrame))
        {
            SceneManager.LoadScene("Menu_Personagem", LoadSceneMode.Single);
        } else if (Keyboard.current.escapeKey.wasReleasedThisFrame 
            || ( Gamepad.current != null 
            &&   Gamepad.current.selectButton.wasReleasedThisFrame))
            {
                Debug.Log("Quit Game");
                Application.Quit();
            }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalScreenMenu : MonoBehaviour
{
    public static bool gameIsFinished = false;
    public GameObject finalMenuUI;
    public Text winnerText;
    public Text drawText;

    void Start() {
        Debug.Log("Finalizado:" + FinalScreenMenu.gameIsFinished + " - Time: " + Time.timeScale);
    }

    void Update()
    {

    }

    public void PlayAgain(){
        gameIsFinished = false;
        finalMenuUI.SetActive(gameIsFinished);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void FinishGame(int winner){
        
        gameIsFinished = true;
        
        switch(winner){
            case 0:
                winnerText.text = "";
                drawText.text = "That's a draw folks!";
                Time.timeScale = 0f;
                finalMenuUI.SetActive(true);
                break;
            case 1:
                winnerText.text = "Player 1 Wins!";
                drawText.text = "";
                Time.timeScale = 0f;
                finalMenuUI.SetActive(true);
                break;
            case 2:
                winnerText.text = "Player 2 Wins!";
                drawText.text = "";
                Time.timeScale = 0f;
                finalMenuUI.SetActive(true);
                break;
        }
    }
}

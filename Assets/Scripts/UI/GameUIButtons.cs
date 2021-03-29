using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIButtons : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void BackButton() 
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("MainMenu");
    }

    public void RefreshButton(bool fromPauseMenu) 
    {
        if (fromPauseMenu && !gameManager.countdownInProgress)
            gameManager.RestartGame(fromPauseMenu);
        else if (!fromPauseMenu)
            gameManager.RestartGame();
    }

    public void SettingsButton() 
    {

    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIButtons : MonoBehaviour
{
    public bool multiplayer;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void BackButton(GameObject parent = null)
    {
        Time.timeScale = 1;

        if (!multiplayer)
            SceneManager.LoadScene("MainMenu");
        else
        {
            if (MultiplayerPaddleSetup.paddleID == 1)
                SceneManager.LoadScene("MainMenu");
            else
                parent.SetActive(false);
        }
    }

    public void RefreshButton(bool fromPauseMenu)
    {
        if (fromPauseMenu && !gameManager.countdownInProgress)
            gameManager.RestartGame(fromPauseMenu);
        else if (!fromPauseMenu)
            gameManager.RestartGame();
    }
}

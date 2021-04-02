using UnityEngine;
using UnityEngine.UI;

public class UIAnimationManager : MonoBehaviour
{
    #region PressAnyKeyUI
    public GameObject[] pressAnyKeyUI;

    public Text[] player1ControlUI;
    public Image[] player2ControlUI;

    public void HideUIKeys()
    {
        //Deactivate 'PressAnyKey' Reminder Text
        pressAnyKeyUI[0].SetActive(false);

        for (int i = 0; i < pressAnyKeyUI.Length; i++)
        {
            pressAnyKeyUI[i].GetComponent<Animator>().SetBool("FadeOut", true);
        }
    }

    public void TransitionKeyUIColor(bool player1)
    {
        if (player1)
        {
            for (int i = 1; i < 5; i++)
            {
                pressAnyKeyUI[i].GetComponent<Animator>().SetBool("KeyPressed", true);
            }
        }
        else if (pressAnyKeyUI.Length > 5)
        {
            for (int i = 5; i < 9; i++)
            {
                pressAnyKeyUI[i].GetComponent<Animator>().SetBool("KeyPressed", true);
            }
        }
    }
    #endregion

    public GameObject[] pauseMenuUI;

    public void PauseMenuFadeOut()
    {
        for (int i = 0; i < pauseMenuUI.Length; i++)
        {
            pauseMenuUI[i].GetComponent<Animator>().SetBool("FadeOut", true);
        }
    }

    public void DeactivatePauseMenu()
    {
        pauseMenuUI[0].SetActive(false);
    }

    public GameObject[] settingsMenuUI;

    public void SettingsMenuFadeOut()
    {
        for (int i = 0; i < settingsMenuUI.Length; i++)
        {
            settingsMenuUI[i].GetComponent<Animator>().SetBool("FadeOut", true);
        }
    }

    public void DeactivateSettingsMenu()
    {
        settingsMenuUI[0].SetActive(false);
    }
}

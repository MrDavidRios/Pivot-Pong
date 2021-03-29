using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchBetweenModes : MonoBehaviour
{
    //Strings
    public static string gamemode;

    //GameObjects
    public GameObject TimedContainer;
    public GameObject RoundsContainer;

    //Dropdown Menu
    public TMP_Dropdown gamemodeDropdown;

    //Gamemode Display Text
    public TMP_Text gamemodeDisplayText;

    public void DecideGamemodes()
    {
        switch (gamemodeDropdown.value)
        {
            case 0:
                //Timed
                gamemode = "Timed";

                ActivateComponents(true, false);
                break;
            case 1:
                //Rounds
                gamemode = "Rounds";

                ActivateComponents(false, true);
                break;
            default:
                Debug.LogWarning("Invalid Gamemode Inputted: " + gamemodeDropdown.value);
                break;
        }

        GameSetup.UpdateGamemode(gamemode);
        gamemodeDisplayText.text = "Gamemode: " + gamemode;
    }

    private void ActivateComponents(bool bool1, bool bool2)
    {
        TimedContainer.SetActive(bool1);
        RoundsContainer.SetActive(bool2);
    }
}

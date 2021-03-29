using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    #region Initialization
    //Booleans
    //bool ballSpeedIncrease;
    //bool paddleSpeedIncrease;

    //Integers
    public static int scoreLimit;
    public static int[] timeLimitInfo;
    public static int roundAmount;

    //Strings
    public static string gamemode;

    //Gamemode Dropdown
    //public TMP_Dropdown gamemodeDropdown;
    public TMP_InputField roundAmountInputField;
    public TMP_InputField[] timeAmountInputFields;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        GetComponent<Settings>().LoadSettings();
    }

    public void UpdateGameInfo()
    {
        switch (gamemode)
        {
            case "Timed":
                timeLimitInfo = new int[] { Int32.Parse(timeAmountInputFields[0].text), Int32.Parse(timeAmountInputFields[1].text) };
                break;
            case "Rounds":
                roundAmount = Int32.Parse(roundAmountInputField.text);
                break;
            default:
                return;
        }
    }

    public static void UpdateGamemode(string _gamemode)
    {
        gamemode = _gamemode;
    }
}

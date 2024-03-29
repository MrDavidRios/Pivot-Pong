﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public bool onlineMultiplayer;

    public bool _hosting;

    public static bool hosting;

    //Gamemode Dropdown
    //public TMP_Dropdown gamemodeDropdown;
    public TMP_InputField roundAmountInputField;
    public TMP_InputField[] timeAmountInputFields;
    #endregion

    public void UpdateGameInfo()
    {
        switch (gamemode)
        {
            case "Timed":
                string inputtedMinute = timeAmountInputFields[0].text;
                string inputtedSecond = timeAmountInputFields[1].text;
                timeLimitInfo = new int[] { int.Parse(inputtedMinute == "" ? "0" : inputtedMinute), int.Parse(inputtedSecond == "" ? "0" : inputtedSecond) };
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

    public void SetMultiplayerStatus(bool multiplayerStatus) => onlineMultiplayer = multiplayerStatus;

    public void SetHostingStatus(bool hostingStatus)
    {
        _hosting = hostingStatus;
        hosting = hostingStatus;
    }

    public void JoinSceneBasedOnMultiplayerStatus()
    {
        FindObjectOfType<SceneSwitcher>().FadeToLevel(onlineMultiplayer ? "2PlayerSceneMultiplayer" : "2PlayerScene");
    }

    public void ActivateObject(GameObject obj) => obj.SetActive(true);
    public void DeactivateObject(GameObject obj) => obj.SetActive(false);
}

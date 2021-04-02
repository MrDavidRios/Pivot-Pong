using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportScore : MonoBehaviour
{
    //Booleans
    public bool multiplayer;

    //Strings
    public string goalName;

    //Scripts
    private dynamic gameManager;

    private void Awake()
    {
        if (multiplayer)
            gameManager = FindObjectOfType<MultiplayerGameManager>();
        else
            gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Game Manager: " + gameManager);
        if (multiplayer && gameManager == null)
            gameManager = FindObjectOfType<MultiplayerGameManager>();

        if (goalName == "LeftGoal")
            gameManager.Score(false, 1);
        else if (goalName == "RightGoal")
            gameManager.Score(true, 1);
    }
}
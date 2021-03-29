using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportScore : MonoBehaviour
{
    //Strings
    public string goalName;

    //Scripts
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (goalName == "LeftGoal")
            gameManager.Score(false, 1);
        else if (goalName == "RightGoal")
            gameManager.Score(true, 1);
    }
}
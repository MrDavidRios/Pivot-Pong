using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //Floats
    private float leftBoundXValue;
    private float rightBoundXValue;

    //Strings
    public string currentMenuPage;
    public static string currentMenuPageStatic;

    //Transforms
    private Transform ball;
    public Transform ballServePosition;

    //GameObjects/UI
    public GameObject[] cameraPositions;
    public GameObject[] timeInputFields;
    public GameObject roundInputField;

    public Button completionVerifyButton;

    //Scripts
    private BallServe ballServe;
    private CameraTransition cameraTransition;
    private SwitchMenuWindows switchMenuWindows;

    void Awake()
    {
        currentMenuPageStatic = "MainMenu";

        ball = GameObject.Find("Ball").transform;
        ballServe = FindObjectOfType<BallServe>();

        cameraTransition = FindObjectOfType<CameraTransition>();
        switchMenuWindows = FindObjectOfType<SwitchMenuWindows>();

        ballServe.ServeBall();

        leftBoundXValue = Camera.main.ScreenToWorldPoint(Vector2.zero).x;
        rightBoundXValue = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
    }

    private void Update()
    {
        currentMenuPage = currentMenuPageStatic;

        if (ball.position.x < leftBoundXValue || ball.position.x > rightBoundXValue)
        {
            ballServe.RepositionBall(ballServePosition);
            ballServe.ServeBall();
        }

        /*Extra Actions*/

        //Make the user be unable to proceed to the game if they leave the time/rounds fields empty.
        if (switchMenuWindows.transitionVerificationCompleted && currentMenuPage == "GameFinalSetup")
        {
            if (timeInputFields[0].activeInHierarchy)
            {
                completionVerifyButton.GetComponent<Tooltip>().tooltip.GetComponent<TMP_Text>().text = "Matches must be 30 seconds or longer.";

                int minutesResult = 0;
                int secondsResult = 0;

                string minutesInputText = timeInputFields[0].GetComponent<TMP_InputField>().text;
                string secondsInputText = timeInputFields[1].GetComponent<TMP_InputField>().text;

                bool successMinutes = int.TryParse(minutesInputText == "" ? "0" : minutesInputText, out minutesResult);
                bool successSeconds = int.TryParse(secondsInputText == "" ? "0" : secondsInputText, out secondsResult);

                bool validTime = false;

                if (successMinutes && successSeconds)
                {
                    if (minutesResult == 0 && secondsResult < 30)
                        validTime = false;
                    else if (minutesResult > 0 && secondsResult < 30 || secondsResult > 30)
                        validTime = true;
                }

                if (validTime)
                {
                    completionVerifyButton.GetComponent<Tooltip>().displayTooltip = false;

                    completionVerifyButton.interactable = true;
                }
                else
                {
                    completionVerifyButton.GetComponent<Tooltip>().displayTooltip = true;

                    completionVerifyButton.interactable = false;
                }
            }
            else if (roundInputField.activeInHierarchy)
            {
                completionVerifyButton.GetComponent<Tooltip>().tooltip.GetComponent<TMP_Text>().text = "You need to enter a round amount to continue.";

                if (roundInputField.GetComponent<TMP_InputField>().text == "")
                {
                    completionVerifyButton.GetComponent<Tooltip>().displayTooltip = true;
                    completionVerifyButton.interactable = false;
                }
                else
                {
                    completionVerifyButton.GetComponent<Tooltip>().displayTooltip = false;
                    completionVerifyButton.interactable = true;
                }
            }
        }

        //Make the left arrow key act as back arrow in menu windows
        if (Input.GetKeyDown(KeyCode.LeftArrow) && switchMenuWindows.transitionVerificationCompleted)
        {
            switch (currentMenuPage)
            {
                case "ModeSelector":
                    cameraTransition.Transition(cameraPositions[0]); //Go to main menu
                    switchMenuWindows.SwitchingTo("MainMenu"); //Update the switch menu script with the correct menu that is being switched to (Main Menu).
                    break;
                case "GameSetup":
                    cameraTransition.Transition(cameraPositions[1]); //Go to mode selector menu
                    switchMenuWindows.SwitchingTo("ModeSelector"); //Update the switch menu script with the correct menu that is being switched to (Main Menu).
                    break;
                case "GameFinalSetup":
                    cameraTransition.Transition(cameraPositions[2]); //Go to game setup menu
                    switchMenuWindows.SwitchingTo("GameSetup"); //Update the switch menu script with the correct menu that is being switched to (Main Menu).
                    break;
                default:
                    break;
            }
        }

        //Press 'play' button on main menu screen with the Enter keys (keyboard & numpad) 
        if (currentMenuPage == "MainMenu" && (Input.GetKeyDown(KeyCode.Return /*Keyboard Enter*/) || Input.GetKeyDown(KeyCode.KeypadEnter /*Numeric Keypad Enter*/)) && switchMenuWindows.transitionVerificationCompleted)
        {
            cameraTransition.Transition(cameraPositions[1]); //Go to mode selector menu
            switchMenuWindows.SwitchingTo("ModeSelector"); //Update the switch menu script with the correct menu that is being switched to (Mode Selector Menu).
        }

        //Select the second time input field if the first input field is selected and Tab is pressed.
        if (currentMenuPage == "GameFinalSetup" && SwitchBetweenModes.gamemode == "Timed" && timeInputFields[0].GetComponent<TMP_InputField>().IsActive() && Input.GetKeyDown(KeyCode.Tab))
            timeInputFields[1].GetComponent<TMP_InputField>().ActivateInputField();
    }
}
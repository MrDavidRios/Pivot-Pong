using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwitchMenuWindows : MonoBehaviour
{
    #region Initialization
    //Strings
    private string currentSceneName;

    public string startingSceneName;

    public bool transitionVerificationCompleted;

    //Transforms
    public Transform ballServePosition;

    //Scripts
    private MainMenu menuManager;
    private BallServe ballServe;
    private ScalePaddles scalePaddles;
    private CameraTransition cameraTransition;

    private void Awake()
    {
        currentSceneName = startingSceneName;
        transitionVerificationCompleted = true;

        menuManager = FindObjectOfType<MainMenu>();
        ballServe = FindObjectOfType<BallServe>();
        scalePaddles = FindObjectOfType<ScalePaddles>();
        cameraTransition = FindObjectOfType<CameraTransition>();
    }
    #endregion

    #region BeforeTransition
    public void SwitchingTo(string _sceneName)
    {
        //Before Transition Functions
        //Update this code every time a new menu window is added.

        switch (_sceneName)
        {
            //Destination Scene
            case "MainMenu":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "ModeSelector":
                        break;
                    case "SettingsMenu":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "SettingsMenu":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "MainMenu":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "ModeSelector":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "MainMenu":
                        ballServe.RepositionBall(ballServePosition);
                        scalePaddles.enabled = false;
                        break;
                    case "GameSetup":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "GameSetup":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "ModeSelector":
                        break;
                    case "GameFinalSetup":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "GameFinalSetup":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "GameSetup":
                        menuManager.timeInputFields[0].GetComponent<TMP_InputField>().ActivateInputField();
                        break;
                    case "ColorPicker":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "ColorPicker":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "GameFinalSetup":
                        break;
                }
                break;
            default:
                LogInvalidSceneNameError(_sceneName);
                break;
        }

        StartCoroutine(VerifyTransitionCompletion(_sceneName));
    }
    #endregion

    #region AfterTransition
    private void RunNecessaryFunctionsAfterTransition(string _sceneName)
    {
        //After Transition Functions
        //Update this code every time a new menu window is added.

        switch (_sceneName)
        {
            //Destination Scene
            case "MainMenu":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "ModeSelector":
                        ballServe.RepositionBall(ballServePosition);
                        ballServe.ServeBall();
                        scalePaddles.enabled = true;
                        break;
                    case "SettingsMenu":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "SettingsMenu":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "MainMenu":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "ModeSelector":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "MainMenu":
                        break;
                    case "GameSetup":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "GameSetup":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "ModeSelector":
                        break;
                    case "GameFinalSetup":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "GameFinalSetup":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "GameSetup":
                        break;
                    case "ColorPicker":
                        break;
                    default:
                        LogInvalidSceneNameError(_sceneName);
                        break;
                }
                break;
            case "ColorPicker":
                switch (currentSceneName)
                {
                    //Current Scene
                    case "GameFinalSetup":
                        break;
                }
                break;
            default:
                LogInvalidSceneNameError(_sceneName);
                break;
        }

        currentSceneName = _sceneName;

        transitionVerificationCompleted = true;
    }
    #endregion

    IEnumerator VerifyTransitionCompletion(string _sceneName)
    {
        transitionVerificationCompleted = false;
        yield return new WaitUntil(() => cameraTransition.transitionComplete);
        RunNecessaryFunctionsAfterTransition(_sceneName);
    }

    #region Debug (Error prevention)
    private void LogInvalidSceneNameError(string _sceneName)
    {
        Debug.LogWarning("Scene Name " + _sceneName + " not currently registered.");
    }
    #endregion
}
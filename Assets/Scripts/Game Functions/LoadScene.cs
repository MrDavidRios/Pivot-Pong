using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //Booleans
    private bool transitionComplete;

    //Strings
    private string sceneName;
    public string startingSceneName;

    //Transforms
    public Transform ballServePosition;
    
    //GameObjects
    public GameObject cameraWrapper;
    public GameObject fadePanel;

    //Scripts
    private BallServe ballServe;
    private ScalePaddles scalePaddles;

    private void Awake()
    {
        sceneName = startingSceneName;

        ballServe = FindObjectOfType<BallServe>();
        scalePaddles = FindObjectOfType<ScalePaddles>();
    }

    public void SwitchScene(string _sceneName)
    {
        //_sceneName is the scene we're switching to; sceneName is the scene we're currently on.

        if (sceneName == _sceneName)
            return;

        if (sceneName == "MainMenu")
        {
            ballServe.RepositionBall(ballServePosition);
            scalePaddles.enabled = false;
        }
        else if (_sceneName == "MainMenu")
        {
            ballServe.RepositionBall(ballServePosition);
        }

        StartCoroutine(CompleteSceneSwitchCoroutine(_sceneName));
    }

    IEnumerator CompleteSceneSwitchCoroutine(string _sceneName)
    {
        switch (_sceneName)
        {
            case "ModeSelector":
                cameraWrapper.GetComponent<Animator>().SetBool("SwitchSceneRight", true);
                break;
            case "MainMenu":
                cameraWrapper.GetComponent<Animator>().SetBool("SwitchSceneLeft", true);
                break;
            default:
                Debug.LogWarning("Error: Unknown Scene: " + _sceneName);
                break;
        }

        yield return new WaitUntil(() => transitionComplete);
        yield return new WaitUntil(() => transitionComplete);

        switch (_sceneName)
        {
            case "ModeSelector":
                cameraWrapper.GetComponent<Animator>().SetBool("SwitchSceneRight", false);
                scalePaddles.enabled = false;
                break;
            case "MainMenu":
                cameraWrapper.GetComponent<Animator>().SetBool("SwitchSceneLeft", false);
                scalePaddles.enabled = true;
                break;
            default:
                Debug.LogWarning("Error: Unknown Scene: " + _sceneName);
                break;
        }

        sceneName = _sceneName;

        yield return 0;
    }

    public void SetAnimationCompletionBool(string status)
    {
        Debug.Log(status);
        if (status == "Complete")
            transitionComplete = true;
        else
            transitionComplete = false;
    }
}
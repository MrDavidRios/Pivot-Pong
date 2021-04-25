using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    //Booleans
    public bool transitionComplete;

    //Floats
    public float lerpSpeed;

    //GameObjects
    public GameObject inputBlockPanel;

    private void Awake() 
    {
        transitionComplete = true;
    }

    public void Transition(GameObject newCameraPosObj)
    {
        if (!transitionComplete)
            return;

        StartCoroutine(TransitionCoroutine(newCameraPosObj.transform.position));

        MainMenu.currentMenuPageStatic = newCameraPosObj.name.Substring(0, newCameraPosObj.name.Length - 8);
    }

    private IEnumerator TransitionCoroutine(Vector3 newCameraPos)
    {
        float t = 0;

        inputBlockPanel.SetActive(true);

        transitionComplete = false;

        while (t <= 1f)
        {
            t += Time.deltaTime * lerpSpeed;

            if (Vector2.Distance(transform.position, newCameraPos) < 0.01f)
            {
                transform.position = newCameraPos;

                break;
            }

            transform.position = Vector2.Lerp(transform.position, newCameraPos, t);

            yield return null;
        }

        inputBlockPanel.SetActive(false);

        transitionComplete = true;
    }
}
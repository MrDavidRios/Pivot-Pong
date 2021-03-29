using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeChange : MonoBehaviour
{
    //GameObjects
    public GameObject[] primaryColorObjects;
    public GameObject[] invertedColorObjects;

    //Colors
    public Color primaryColor;
    public Color invertedColor;
    public Color backgroundColor;

    //Floats
    public float lerpSpeed;
    public float backgroundLerpSpeed;

    //Main Camera
    public Camera mainCamera;

    public void ChangeColorScheme()
    {
        StartCoroutine(ChangeColorSchemeCoroutine());
    }

    IEnumerator ChangeColorSchemeCoroutine()
    {
        StartCoroutine(ChangeBackgroundColor());

        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime * lerpSpeed;

            foreach (GameObject obj in primaryColorObjects)
            {
                if (obj.GetComponent<Text>() == null && obj.GetComponent<Image>() == null)
                    obj.GetComponent<SpriteRenderer>().color = Color.Lerp(obj.GetComponent<SpriteRenderer>().color, primaryColor, t);
                else if (obj.GetComponent<Image>() == null)
                    obj.GetComponent<Text>().color = Color.Lerp(obj.GetComponent<Text>().color, primaryColor, t);
                else
                    obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, primaryColor, t);
            }

            foreach (GameObject obj in invertedColorObjects)
            {
                if (obj.GetComponent<Text>() == null && obj.GetComponent<Image>() == null)
                    obj.GetComponent<SpriteRenderer>().color = Color.Lerp(obj.GetComponent<SpriteRenderer>().color, invertedColor, t);
                else if (obj.GetComponent<Image>() == null)
                    obj.GetComponent<Text>().color = Color.Lerp(obj.GetComponent<Text>().color, invertedColor, t);
                else
                    obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, invertedColor, t);
            }

            yield return null;
        }
    }

    IEnumerator ChangeBackgroundColor()
    {
        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime * backgroundLerpSpeed;

            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, backgroundColor, t);

            yield return null;
        }
    }
}
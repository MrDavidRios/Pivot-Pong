using System.Collections;
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

    private IEnumerator ChangeColorSchemeCoroutine()
    {
        StartCoroutine(ChangeBackgroundColor());

        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime * lerpSpeed;

            foreach (GameObject obj in primaryColorObjects)
            {
                if (obj.GetComponent<SpriteRenderer>() != null)
                    obj.GetComponent<SpriteRenderer>().color =
                        Color.Lerp(obj.GetComponent<SpriteRenderer>().color, primaryColor, t);
                else if (obj.GetComponent<Text>() != null)
                    obj.GetComponent<Text>().color = Color.Lerp(obj.GetComponent<Text>().color, primaryColor, t);
                else if (obj.GetComponent<Image>() != null)
                    obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, primaryColor, t);
                else if (obj.GetComponent<TrailRenderer>() != null)
                {
                    //GradientColorKey[] colorKeys = GenerateTrailGradient(invertedColor, Color.white, t);

                    obj.GetComponent<TrailRenderer>().colorGradient =
                        GenerateTrailGradient(obj.GetComponent<TrailRenderer>().colorGradient, invertedColor,
                            Color.white, t);

                    //obj.GetComponent<TrailRenderer>().startColor = Color.Lerp(obj.GetComponent<TrailRenderer>().startColor, invertedColor, t);
                }
            }

            foreach (GameObject obj in invertedColorObjects)
            {
                if (obj.GetComponent<Text>() == null && obj.GetComponent<Image>() == null)
                    obj.GetComponent<SpriteRenderer>().color =
                        Color.Lerp(obj.GetComponent<SpriteRenderer>().color, invertedColor, t);
                else if (obj.GetComponent<Image>() == null)
                    obj.GetComponent<Text>().color = Color.Lerp(obj.GetComponent<Text>().color, invertedColor, t);
                else
                    obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, invertedColor, t);
            }

            yield return null;
        }
    }

    private IEnumerator ChangeBackgroundColor()
    {
        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime * backgroundLerpSpeed;

            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, backgroundColor, t);

            yield return null;
        }
    }

    private static Gradient GenerateTrailGradient(Gradient trailGradient, Color baseColor, Color trailColor, float t)
    {
        var gradient = new Gradient();

        //Slightly lighter than base color to help transition to white
        Color baseTailColor = Utils.ModifyColors.ChangeColorBrightness(baseColor, 0.2f);
        Color centerColor = Utils.ModifyColors.ChangeColorBrightness(baseColor, 0.5f);

        gradient.SetKeys(
            new[]
            {
                new GradientColorKey(baseTailColor, t),
                new GradientColorKey(centerColor, t),
                trailGradient.colorKeys[2]
            },
            trailGradient.alphaKeys);

        return gradient;
    }
}
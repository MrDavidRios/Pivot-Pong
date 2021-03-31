using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ColorScheme
{
    public Color primaryColor;
    public Color secondaryColor;
    public Color backgroundColor;
}

public class ColorSchemeChange : MonoBehaviour
{
    //GameObjects
    public GameObject[] primaryColorObjects;
    public GameObject[] secondaryColorObjects;

    //Colors
    public ColorScheme[] colorSchemes;

    //Booleans
    private bool currentlyLerping = false;
    private bool anotherLerpStarted = false;

    //Integers
    private int currentStageIndex = 0;
    private int stageIndexToCutOff = 0;

    //Floats
    public float lerpSpeed;
    public float backgroundLerpSpeed;

    //Main Camera
    public Camera mainCamera;

    public void ChangeColorScheme(int stageIndex)
    {
        //Don't set the color scheme to default if it is already set to the default color scheme.
        if (stageIndex != currentStageIndex)
        {
            if (currentlyLerping)
            {
                anotherLerpStarted = true;
                stageIndexToCutOff = currentStageIndex;
            }

            StartCoroutine(ChangeColorSchemeCoroutine(stageIndex));
        }
    }

    /// Call this whenever the ball exceeds a certain speed
    private IEnumerator ChangeColorSchemeCoroutine(int stageIndex)
    {
        StartCoroutine(ChangeBackgroundColor(stageIndex));

        currentStageIndex = stageIndex;

        float t = 0;

        while (t <= 1f)
        {
            currentlyLerping = true;

            if (anotherLerpStarted && stageIndex == stageIndexToCutOff) //Cut off lerp if another one is requested.
            {
                anotherLerpStarted = false;

                yield break;
            }

            t += Time.deltaTime * lerpSpeed;

            foreach (GameObject obj in primaryColorObjects) //TODO: Optimize this so that it uses generics to access components instead of GetComponent<>. This should shave off some ms.
            {
                if (obj.GetComponent<SpriteRenderer>() != null)
                    obj.GetComponent<SpriteRenderer>().color =
                        Color.Lerp(obj.GetComponent<SpriteRenderer>().color, colorSchemes[stageIndex].primaryColor, t);
                else if (obj.GetComponent<Text>() != null)
                    obj.GetComponent<Text>().color = Color.Lerp(obj.GetComponent<Text>().color, colorSchemes[stageIndex].primaryColor, t);
                else if (obj.GetComponent<Image>() != null)
                    obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, colorSchemes[stageIndex].primaryColor, t);
                else if (obj.GetComponent<TrailRenderer>() != null)
                {
                    obj.GetComponent<TrailRenderer>().colorGradient =
                        GenerateTrailGradient(obj.GetComponent<TrailRenderer>().colorGradient, colorSchemes[stageIndex].secondaryColor,
                            Color.white, t);
                }
            }

            foreach (GameObject obj in secondaryColorObjects)
            {
                if (obj.GetComponent<Text>() == null && obj.GetComponent<Image>() == null)
                    obj.GetComponent<SpriteRenderer>().color =
                        Color.Lerp(obj.GetComponent<SpriteRenderer>().color, colorSchemes[stageIndex].secondaryColor, t);
                else if (obj.GetComponent<Image>() == null)
                    obj.GetComponent<Text>().color = Color.Lerp(obj.GetComponent<Text>().color, colorSchemes[stageIndex].secondaryColor, t);
                else
                    obj.GetComponent<Image>().color = Color.Lerp(obj.GetComponent<Image>().color, colorSchemes[stageIndex].secondaryColor, t);
            }

            yield return null;
        }

        currentlyLerping = false;
        anotherLerpStarted = false;
    }

    private IEnumerator ChangeBackgroundColor(int stageIndex)
    {
        float t = 0;

        while (t <= 1f)
        {
            if (anotherLerpStarted && stageIndex == stageIndexToCutOff) //Cut off lerp if another one is requested.
                yield break;

            t += Time.deltaTime * backgroundLerpSpeed;

            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, colorSchemes[stageIndex].backgroundColor, t);

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
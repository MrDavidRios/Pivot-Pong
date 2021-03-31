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

    //Integers
    private int currentStageIndex = 0;

    //Floats
    public float lerpSpeed;
    public float backgroundLerpSpeed;

    //Main Camera
    public Camera mainCamera;

    public void ChangeColorScheme(int stageIndex)
    {
        StartCoroutine(ChangeColorSchemeCoroutine(stageIndex));
    }

    /// Call this whenever the ball exceeds a certain speed
    private IEnumerator ChangeColorSchemeCoroutine(int stageIndex)
    {
        //Don't set the color scheme to default if it is already set to the default color scheme.
        if (stageIndex == currentStageIndex && stageIndex == 0)
            yield break;

        StartCoroutine(ChangeBackgroundColor(colorSchemes[stageIndex].backgroundColor));

        currentStageIndex = stageIndex;

        float t = 0;

        Debug.Log("Here! Stage: " + stageIndex);

        while (t <= 1f)
        {
            t += Time.deltaTime * lerpSpeed;

            foreach (GameObject obj in primaryColorObjects)
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
                    Debug.Log(colorSchemes[stageIndex].secondaryColor + "; Stage Index: " + stageIndex + "; t: " + t);

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

        Debug.Log("Stage " + (stageIndex + 1) + " Complete.");
    }

    private IEnumerator ChangeBackgroundColor(Color newBackgroundColor)
    {
        float t = 0;

        while (t <= 1f)
        {
            t += Time.deltaTime * backgroundLerpSpeed;

            mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, newBackgroundColor, t);

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
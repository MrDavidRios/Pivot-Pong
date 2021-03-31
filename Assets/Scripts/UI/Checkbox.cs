using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
    //Integers
    public int settingIndex;

    //Vector3
    private Vector3 originalPosition;

    //Sprites
    public Sprite checkboxFull;
    public Sprite checkboxEmpty;

    //Scripts
    private Settings settings;

    private void Awake()
    {
        settings = FindObjectOfType<Settings>();

        originalPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    private void Update()
    {
        if (settings.settings[settingIndex])
        {
            GetComponent<Image>().sprite = checkboxFull;
            GetComponent<RectTransform>().localScale = new Vector3(1.25f, 1.25f, 1f);

            GetComponent<RectTransform>().anchoredPosition = new Vector2(originalPosition.x + 2.5f, originalPosition.y + 2.5f);
        }
        else
        {
            GetComponent<Image>().sprite = checkboxEmpty;
            GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
    }

    public void CheckboxClicked()
    {
        switch (settingIndex)
        {
            case 0:
                settings.EnableAdaptiveColor(!settings.adaptiveColor);
                break;
            case 1:
                settings.EnableCameraShake(!settings.cameraShakeEnabled);
                break;
            case 2:
                settings.EnableAutoReServe(!settings.autoReServe);
                break;
            case 3:
                settings.EnableBallTail(!settings.ballTail);
                break;
            default:
                Debug.LogError("Invalid setting index: " + settingIndex);
                break;
        }
    }
}

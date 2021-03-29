using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextInputTimeValue : MonoBehaviour
{
    //Booleans
    public bool allowNegatives;

    public bool minutes;

    //Integers
    public int timeChangeValue;

    //Strings
    public string defaultValue;

    //Scripts
    private RectTransformPositionCorrection positionOverrideScript;

    public void AddTime()
    {
        if (GetComponent<TMP_InputField>().text == "")
            GetComponent<TMP_InputField>().text = "" + defaultValue;

        int textInputFieldValue = int.Parse(GetComponent<TMP_InputField>().text);

        GetComponent<TMP_InputField>().text = "" + (textInputFieldValue + timeChangeValue);
        positionOverrideScript.CorrectPositionTop(minutes, false);
    }

    public void SubtractTime()
    {
        if (GetComponent<TMP_InputField>().text == "")
            return;

        int textInputFieldValue = int.Parse(GetComponent<TMP_InputField>().text);

        if (!allowNegatives && textInputFieldValue == 0)
            return;

        GetComponent<TMP_InputField>().text = "" + (textInputFieldValue - timeChangeValue);
        positionOverrideScript.CorrectPositionTop(minutes, false);
    }

    private void Awake()
    {
        positionOverrideScript = FindObjectOfType<RectTransformPositionCorrection>();
    }
}

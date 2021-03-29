using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FormatInputAsTime : MonoBehaviour
{
    public bool minuteInputField;

    private RectTransformPositionCorrection positionOverrideScript;

    private void Awake()
    {
        positionOverrideScript = FindObjectOfType<RectTransformPositionCorrection>();
    }

    private void Update()
    {
        if (GetComponent<TMP_InputField>().text.Length == 1 && !GetComponent<TMP_InputField>().isFocused)
        {
            GetComponent<TMP_InputField>().text = "0" + GetComponent<TMP_InputField>().text;

            positionOverrideScript.CorrectPositionTop(minuteInputField, false);
        }
    }
}

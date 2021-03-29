using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextInputValue : MonoBehaviour
{
    /// <summary>
    /// Attach this script to the text input field being used on.
    /// 
    /// Only works with integers!
    /// </summary>

    //Booleans
    public bool negativesAllowed;
    public bool zeroesAllowed;

    public bool minutes;
    public bool rounds;

    //Integers
    public int valueChangeAmount;
    public int defaultValue;

    //Floats
    public float textTopValue;
    public float textBottomValue;

    //RectTransform
    public RectTransform text;

    //Scripts
    private RectTransformPositionCorrection positionOverrideScript;

    public void AddValue()
    {
        if (GetComponent<TMP_InputField>().text == "")
            SetToDefaultValue();

        int textInputFieldValue = int.Parse(GetComponent<TMP_InputField>().text);
        GetComponent<TMP_InputField>().text = "" + (textInputFieldValue + valueChangeAmount);
        positionOverrideScript.CorrectPositionTop(minutes, rounds);
    }

    public void SubtractValue()
    {
        if (GetComponent<TMP_InputField>().text == "")
            SetToDefaultValue();

        int textInputFieldValue = int.Parse(GetComponent<TMP_InputField>().text);

        if (!zeroesAllowed)
        {
            if (textInputFieldValue == 1)
                return;
        }
        else if (!negativesAllowed)
        {
            if (textInputFieldValue == 0)
                return;
        }

        GetComponent<TMP_InputField>().text = "" + (textInputFieldValue - valueChangeAmount);
        positionOverrideScript.CorrectPositionTop(minutes, rounds);
    }

    private void CorrectTextPosition()
    {
        text.offsetMax = new Vector2(text.offsetMax.x, textTopValue);
        text.offsetMin = new Vector2(text.offsetMin.x, textBottomValue);
    }

    private void SetToDefaultValue()
    {
        GetComponent<TMP_InputField>().text = "" + defaultValue;
        return;
    }

    private void Awake()
    {
        positionOverrideScript = FindObjectOfType<RectTransformPositionCorrection>();
    }

    private void Update()
    {
        if (text.offsetMax.y != textTopValue)
            CorrectTextPosition();
        else if (text.offsetMin.y != textBottomValue)
            CorrectTextPosition();

        if (GetComponent<TMP_InputField>().text == "0")
            AddValue();
    }
}

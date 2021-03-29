using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownIntLimit : MonoBehaviour
{
    //Booleans
    public bool setToDefault;

    //Integers
    public int valueLimit;
    public int defaultValue;

    //Scripts
    private RectTransformPositionCorrection positionOverrideScript;

    private void Awake()
    {
        positionOverrideScript = FindObjectOfType<RectTransformPositionCorrection>();
    }

    private void Update()
    {
        if (GetComponent<TMP_InputField>().text != "")
        {
            int textInputFieldValue = int.Parse(GetComponent<TMP_InputField>().text);

            if (textInputFieldValue > valueLimit)
            {
                if (setToDefault)
                {
                    if (defaultValue == 0)
                    {
                        GetComponent<TMP_InputField>().text = "00";
                        positionOverrideScript.CorrectPositionTop(false, false);
                    }
                    else
                    {
                        GetComponent<TMP_InputField>().text = "" + defaultValue;
                        positionOverrideScript.CorrectPositionTop(false, false);
                    }
                }
                else
                {
                    GetComponent<TMP_InputField>().text = "" + valueLimit;
                    positionOverrideScript.CorrectPositionTop(false, false);
                }
            }
        }
    }
}

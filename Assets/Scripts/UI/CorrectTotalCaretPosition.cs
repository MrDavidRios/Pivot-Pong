using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CorrectTotalCaretPosition : MonoBehaviour
{
    /// <summary>
    /// Attach this script to the "Text Area" GameObject that is the child of the TMP dropdown!
    /// </summary>

    //Booleans
    public bool correctVerticalPosition;
    public bool correctHorizontalPosition;
    
    //Floats
    public float caretTopValue;
    public float caretBottomValue;
    public float caretLeftValue;
    public float caretRightValue;

    public void CorrectCaretPosition(RectTransform caretRectTransform)
    {
        if (correctHorizontalPosition)
        {
            SetLeft(caretRectTransform, caretLeftValue);
            SetRight(caretRectTransform, caretRightValue);
        }

        if (correctVerticalPosition)
        {
            SetTop(caretRectTransform, caretTopValue);
            SetBottom(caretRectTransform, caretBottomValue);
        }
    }

    private void Update()
    {
        for (int x = 0; x < transform.childCount; x++)
        {
            Transform child = transform.GetChild(x);

            if (child.name.Contains("Caret"))
            {
                if (child.GetComponent<RectTransform>().offsetMax.y != caretTopValue)
                    CorrectCaretPosition(child.GetComponent<RectTransform>());
                else if (child.GetComponent<RectTransform>().offsetMin.y != caretBottomValue)
                    CorrectCaretPosition(child.GetComponent<RectTransform>());
            }
        }
    }

    public void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}

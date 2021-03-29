using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RectTransformPositionCorrection : MonoBehaviour
{
    public RectTransform minutesTextArea;
    public RectTransform minutesPlaceholder;

    public RectTransform secondsTextArea;
    public RectTransform secondsPlaceholder;

    public RectTransform roundsTextArea;
    public RectTransform roundsPlaceholder;

    public void CorrectPositionTop(bool minutesInput, bool roundsInput)
    {
        //rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, newPositionValue);
        if (minutesInput && !roundsInput)
        {
            minutesTextArea.offsetMax = new Vector2(minutesTextArea.offsetMax.x, 2);
            minutesPlaceholder.offsetMax = new Vector2(minutesPlaceholder.offsetMax.x, -10);
        }
        else if (!roundsInput)
        {
            secondsTextArea.offsetMax = new Vector2(secondsTextArea.offsetMax.x, 2);
            secondsPlaceholder.offsetMax = new Vector2(secondsPlaceholder.offsetMax.x, -10);
        }

        if (roundsInput)
        {
            roundsTextArea.offsetMax = new Vector2(roundsTextArea.offsetMax.x, 2);
            roundsPlaceholder.offsetMax = new Vector2(roundsPlaceholder.offsetMax.x, -10);
        }
    }
}

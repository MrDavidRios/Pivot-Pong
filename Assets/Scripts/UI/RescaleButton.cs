using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RescaleButton : MonoBehaviour
{
    //Booleans
    private bool enlargeCoroutineStarted;
    private bool shrinkCoroutineStarted;

    private bool enlargeCoroutineComplete;
    private bool shrinkCoroutineComplete;

    //Floats
    public float lerpSpeed;

    //Buttons (RectTransforms)
    public RectTransform selectedButton;

    private List<RectTransform> otherButtons = new List<RectTransform>();

    //GameObjects
    public Transform buttonParent;

    #region OldCode
    /*
    public void Start()
    {
        SetOtherButtons();
    }

    private void SetOtherButtons()
    {
        foreach (Transform buttonChild in buttonParent)
        {
            otherButtons.Add(buttonChild.GetComponent<RectTransform>());
        }
    }

    public void ResetButtons()
    {
        selectedButton = null;

        SetOtherButtons();

        //StartCoroutine(ResetButtonsCoroutine());
    }

    public void EnlargeSelected(RectTransform selectedButtonRectTransform)
    {
        otherButtons.Remove(selectedButtonRectTransform);

        if (selectedButton != null)
            otherButtons.Add(selectedButton);

        selectedButton = selectedButtonRectTransform;

        //StartCoroutine(EnlargeSelectedButton());
    }

    /*
    public void RescaleButtons(bool enlargeSelected, RectTransform selectedButtonRectTransform = null)
    {
        if (selectedButtonRectTransform != null)
        {
            selectedButton = selectedButtonRectTransform;

            foreach (Transform buttonChild in transform)
            {
                if (buttonChild.GetComponent<RectTransform>() != selectedButton)
                    otherButtons.Add(buttonChild.GetComponent<RectTransform>());
            }
        }
        else
        {
            if (enlargeSelected == true)
                return;
        }

        if (enlargeSelected)
            StartCoroutine(EnlargeSelectedButton());
        else
            StartCoroutine(ShrinkSelectedButton());
    }
    

    IEnumerator EnlargeSelectedButton()
    {
        float t = 0;

        while (t <= 1f && selectedButton != null)
        {
            t += Time.deltaTime * lerpSpeed;
            selectedButton.localScale = Vector3.Lerp(selectedButton.localScale, new Vector3(1.1f, 1.1f, 1f), t);

            foreach (RectTransform button in otherButtons)
            {
                button.localScale = Vector3.Lerp(button.localScale, new Vector3(0.9f, 0.9f, 1f), t);
            }

            yield return null;
        }
    }

    IEnumerator ResetButtonsCoroutine()
    {
        float t = 0;

        while (t <= 1f && selectedButton == null)
        {
            foreach (RectTransform button in buttonParent)
            {
                button.localScale = Vector3.Lerp(button.localScale, new Vector3(1f, 1f, 1f), t);
            }

            yield return null;
        }
    }*/
    #endregion
}
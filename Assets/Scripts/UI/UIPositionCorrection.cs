using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HorizontalScreenSide
{
    Left, Right
}

public enum VerticalScreenSide
{
    Bottom, Top
}

public class UIPositionCorrection : MonoBehaviour
{
    #region Initialization
    //Booleans
    public bool visibleDuringTransition;
    public bool constantPosition;

    //Floats
    public float horizontalDistanceFromEdge;
    public float verticalDistanceFromEdge;
    public float desiredZPosition;

    //Vector3
    private Vector3 savedPosition;

    //Screen Side
    public HorizontalScreenSide horizontalButtonScreenSide;
    public VerticalScreenSide verticalButtonScreenSide;

    //RectTransform
    private RectTransform rectTransform;

    //Canvas
    public Canvas desiredCanvas;

    //Scripts
    private CameraTransition cameraTransition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        cameraTransition = FindObjectOfType<CameraTransition>();

        savedPosition = Vector3.zero;
    }
    #endregion

    public void Update()
    {
        //If the given element is visible and a constant position is not desired, then this code will run.
        if (desiredCanvas.GetComponent<IsInViewCheckerNoAnimator>().IsVisible() && !constantPosition)
        {
            //If it is desired that the element be visible during transition and the camera transition has not completed, exit this if statement.
            if (visibleDuringTransition && !cameraTransition.transitionComplete)
                return;

            if (horizontalButtonScreenSide == HorizontalScreenSide.Left) //Left Side
            {
                if (verticalButtonScreenSide == VerticalScreenSide.Bottom) //Bottom
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(horizontalDistanceFromEdge, verticalDistanceFromEdge));
                else //Top
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(horizontalDistanceFromEdge, Screen.height - verticalDistanceFromEdge));
            }
            else //Right Side
            {
                if (verticalButtonScreenSide == VerticalScreenSide.Bottom) //Bottom
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - horizontalDistanceFromEdge, verticalDistanceFromEdge));
                else //Top
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - horizontalDistanceFromEdge, Screen.height - verticalDistanceFromEdge));
            }
        }
        else if (constantPosition) //If the user wants a constant position for this UI element, run this code.
        {
            if (savedPosition != Vector3.zero)
            {
                if (rectTransform.position.x != savedPosition.x && rectTransform.position.y != savedPosition.y)
                    rectTransform.position = savedPosition;

                return;
            }

            if (horizontalButtonScreenSide == HorizontalScreenSide.Left) //Left Side
            {
                if (verticalButtonScreenSide == VerticalScreenSide.Bottom) //Bottom
                {
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).x - (Screen.width / 2) + horizontalDistanceFromEdge,
                                                                                        Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).y - (Screen.height / 2) + verticalDistanceFromEdge));
                }
                else //Top
                {
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).x - (Screen.width / 2) + horizontalDistanceFromEdge,
                                                                                        Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).y + (Screen.height / 2) - verticalDistanceFromEdge));
                }

                savedPosition = rectTransform.position;
            }
            else //Right Side
            {
                if (verticalButtonScreenSide == VerticalScreenSide.Bottom) //Bottom
                {
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).x + (Screen.width / 2) - horizontalDistanceFromEdge,
                                                                                      Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).y - (Screen.height / 2) + verticalDistanceFromEdge));
                }
                else //Top
                {
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).x + (Screen.width / 2) - horizontalDistanceFromEdge,
                                                                                        Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).y + (Screen.height / 2) - verticalDistanceFromEdge));
                }

                savedPosition = rectTransform.position;
            }

            /*
            switch (horizontalButtonScreenSide)
            {
                case HorizontalScreenSide.Left:
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).x - (Screen.width / 2) + horizontalDistanceFromEdge, Camera.main.WorldToScreenPoint(rectTransform.position).y));
                    savedPosition = rectTransform.position;
                    break;
                case HorizontalScreenSide.Right:
                    rectTransform.position = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.WorldToScreenPoint(desiredCanvas.GetComponent<RectTransform>().localPosition).x + (Screen.width / 2) - horizontalDistanceFromEdge, Camera.main.WorldToScreenPoint(rectTransform.position).y));
                    savedPosition = rectTransform.position;
                    break;
                default:
                    Debug.LogWarning("Invalid Screen Side Entered: " + horizontalButtonScreenSide);
                    break;
            }
            */
        }

        rectTransform.position = new Vector3(rectTransform.position.x, rectTransform.position.y, desiredZPosition + 100f);
    }
}
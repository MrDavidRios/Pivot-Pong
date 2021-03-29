using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInViewCheckerNoAnimator : MonoBehaviour
{
    //Camera
    private Camera UICamera;

    private void Awake()
    {
        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    public bool IsVisible()
    {
        Vector3 screenPoint = UICamera.WorldToViewportPoint(transform.position);

        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
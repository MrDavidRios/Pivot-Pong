using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInViewChecker : MonoBehaviour
{
    //Animator
    private Animator anim;

    //Camera
    private Camera UICamera;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    private void Update()
    {
        anim.SetBool("isInView", IsVisible());
    }

    private bool IsVisible()
    {
        Vector3 screenPoint = UICamera.WorldToViewportPoint(transform.position);

        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
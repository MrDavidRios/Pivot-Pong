using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePaddles : MonoBehaviour
{
    //Floats
    public float widthDivisor;

    //GameObjects
    public GameObject paddleLeft;
    public GameObject paddleRight;

    private void Start()
    {
        Vector3 leftPaddlePosValues = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / widthDivisor, Screen.height, 0f));
        Vector3 rightPaddlePosValues = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - (Screen.width / widthDivisor), Screen.height, 0f));

        paddleLeft.transform.position = new Vector2(leftPaddlePosValues.x, paddleLeft.transform.position.y);
        paddleRight.transform.position = new Vector2(rightPaddlePosValues.x, paddleRight.transform.position.y);
    }
}
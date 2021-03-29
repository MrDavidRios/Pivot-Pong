using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    //Transforms
    private Transform ball;
    
    //Floats
    public float paddleSpeed;

    //Booleans
    private bool followMouse = true;
    public bool followMouseEnabled;

    private void Awake()
    {
        ball = GameObject.Find("Ball").transform;
    }

    private void Update()
    {
        float xPos = transform.position.x;

        Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(transform.position, currentMousePosition) > 6.5f)
            followMouse = false;
        else
            followMouse = true;

        if (followMouse)
        {
            if (followMouseEnabled)
            {
                //Follow Cursor
                if (currentMousePosition.y >= -4.5 && currentMousePosition.y <= 4.5)
                    transform.position = Vector2.MoveTowards(transform.position, currentMousePosition, paddleSpeed / 100);

                //Lock paddle to y-axis
                transform.position = new Vector2(xPos, transform.position.y);
            }
            else
            {
                //Follow Ball
                transform.position = Vector2.MoveTowards(transform.position, ball.position, paddleSpeed / 100);

                //Lock paddle to y-axis
                transform.position = new Vector2(xPos, transform.position.y);
            }
        }
        else
        {
            //Follow Ball
            transform.position = Vector2.MoveTowards(transform.position, ball.position, paddleSpeed / 100);

            //Lock paddle to y-axis
            transform.position = new Vector2(xPos, transform.position.y);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallServe : MonoBehaviour
{
    #region Initialization
    //Floats
    public float ballServeForceX;
    public float ballServeForceY;

    private float initBallServeForceX;
    private float initBallServeForceY;

    //Strings
    private string startDir;
    private string startVertDir;

    //Transforms
    public Transform ball;

    //RectTransforms
    public RectTransform directionalArrow;

    //Scripts
    private GameManager gameManager;

    void Awake()
    {
        if (directionalArrow != null)
            directionalArrow.GetComponent<Image>().enabled = false;

        gameManager = GetComponent<GameManager>();

        initBallServeForceX = ballServeForceX;
        initBallServeForceY = ballServeForceY;
    }
    #endregion

    public void PreServeBall()
    {
        GenerateStartDir();

        if (directionalArrow != null)
        {
            if (startDir == "Left")
            {
                if (startVertDir == "Up")
                {
                    directionalArrow.localPosition = new Vector3(-45, 30, 0);
                    directionalArrow.rotation = Quaternion.Euler(0f, 0f, -32.5f);
                }
                else
                {
                    directionalArrow.localPosition = new Vector3(-45, -30, 0);
                    directionalArrow.rotation = Quaternion.Euler(0f, 0f, 32.5f);
                }
            }
            else
            {
                if (startVertDir == "Up")
                {
                    directionalArrow.localPosition = new Vector3(45, 30, 0);
                    directionalArrow.rotation = Quaternion.Euler(0f, 0f, 212.5f);
                }
                else
                {
                    directionalArrow.localPosition = new Vector3(45, -30, 0);
                    directionalArrow.rotation = Quaternion.Euler(0f, 0f, -212.5f);
                }
            }

            directionalArrow.localScale = new Vector3(0.3f, 0.3f, 1f);
            directionalArrow.GetComponent<Image>().enabled = true;
        }
    }

    public void ServeBall(string startDirection = "None")
    {
        if (directionalArrow != null)
            directionalArrow.GetComponent<Image>().enabled = false;

        if (gameManager != null)
        {
            ballServeForceX = initBallServeForceX;
            ballServeForceY = initBallServeForceY;

            float ballForceCoefficient = 1.0f + ((gameManager.player1Score + gameManager.player2Score) / 2 * 0.1f);

            if (ballForceCoefficient > 2.5f)
                ballForceCoefficient = 2.5f;

            ballServeForceX *= ballForceCoefficient;
            ballServeForceY *= ballForceCoefficient;
        }

        if (startDirection == "None")
            startDirection = startDir;

        if (startDir == "Left")
        {
            if (startVertDir == "Up")
                ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-ballServeForceX, ballServeForceY));
            else
                ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(-ballServeForceX, -ballServeForceY));
        }
        else
        {
            if (startVertDir == "Up")
                ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(ballServeForceX, ballServeForceY));
            else
                ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(ballServeForceX, -ballServeForceY));
        }
    }

    public void RepositionBall(Transform newBallPos)
    {
        ball.position = newBallPos.position;

        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    public void GenerateStartDir()
    {
        int decidingInt = Mathf.RoundToInt(Random.Range(0, 5));

        if (decidingInt < 2)
            startDir = "Left";
        else
            startDir = "Right";

        int decidingVertInt = Mathf.RoundToInt(Random.Range(0, 5));

        if (decidingVertInt < 2)
            startVertDir = "Up";
        else
            startVertDir = "Down";
    }
}
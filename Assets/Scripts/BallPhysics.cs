using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class BallPhysics : MonoBehaviour
{
    //Booleans
    private bool reServeCheckStarted = false;
    private bool reServeEnabled = true;

    public bool cameraShakeEnabled;

    //Floats
    public float forceToAddX = 0f;
    public float forceToAddY = 0f;

    public float maxBallXVelocity = 0f;
    public float maxBallYVelocity = 0f;

    public float ballVelocityCoefficient = 0f;

    public float paddleSpeedMultiplier;

    private float leftPaddleHorizontalPosition = 0f;
    private float rightPaddleHorizontalPosition = 0f;

    //Strings
    private string currentSceneName;

    //Rigidbody2D
    private Rigidbody2D rb2d;

    //Scripts
    private GameManager gameManager;
    private PaddleControls paddleControls;
    private Settings settings;
    private AudioManager audioManager;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

        currentSceneName = SceneManager.GetActiveScene().name;

        gameManager = FindObjectOfType<GameManager>();

        settings = FindObjectOfType<Settings>();

        audioManager = FindObjectOfType<AudioManager>();
;
        if (currentSceneName != "MainMenu")
        {
            paddleControls = FindObjectOfType<PaddleControls>();

            leftPaddleHorizontalPosition = paddleControls.player1Paddle.transform.position.x;
            rightPaddleHorizontalPosition = paddleControls.player2Paddle.transform.position.x;
        }
    }

    private void Update()
    {
        ballVelocityCoefficient = Mathf.Abs(rb2d.velocity.x) + Mathf.Abs(rb2d.velocity.y);
        paddleSpeedMultiplier = ballVelocityCoefficient / 15f;

        if (paddleSpeedMultiplier < 1f)
            paddleSpeedMultiplier = 1f;

        reServeEnabled = settings.autoReServe;
        cameraShakeEnabled = settings.cameraShakeEnabled;

        if (currentSceneName != "MainMenu" && !reServeCheckStarted && reServeEnabled)
        {
            StartCoroutine(CheckIfReServe());
        }

        if (Mathf.Abs(rb2d.velocity.x) > maxBallXVelocity)
            forceToAddX = 0;

        if (Mathf.Abs(rb2d.velocity.y) > maxBallYVelocity)
            forceToAddY = 0;
    }

    IEnumerator CheckIfReServe()
    {
        reServeCheckStarted = true;

        if (BallTooSlow(2f, "x"))
        {
            yield return new WaitForSeconds(5);

            if (BallTooSlow(2f, "x"))
                gameManager.InitiateCountdown(true);
        }

        reServeCheckStarted = false;

        yield return null;
    }

    private bool BallTooSlow(float minVelocity, string velocityType)
    {
        //If the ball is 3 units away from either paddle, proceed with the code.
        if (Mathf.Abs(transform.position.x - leftPaddleHorizontalPosition) < 3f && Mathf.Abs(transform.position.x - rightPaddleHorizontalPosition) < 3f)
            return false;

        if (gameManager.countdownInProgress)
            return false;

        Vector2 velocity = new Vector2(Mathf.Abs(rb2d.velocity.x), Mathf.Abs(rb2d.velocity.y));

        if (velocityType == "x")
        {
            if (Mathf.Abs(velocity.x) < minVelocity)
                return true;
            else
                return false;
        }
        else if (velocityType == "y")
        {
            if (Mathf.Abs(velocity.y) < minVelocity)
                return true;
            else
                return false;
        }

        Debug.LogError("Error with velocityType input. Invalid string input.");
        return false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        float yPos = transform.position.y;

        if (col.transform.CompareTag("Paddle"))
        {
            if (col.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                float magnitude = (ballVelocityCoefficient / 3f) * (col.gameObject.GetComponent<Rigidbody2D>().velocity.y / 5f) + 1f;

                if (cameraShakeEnabled)
                    CameraShaker.Instance.ShakeOnce(magnitude, 3f, 0.1f, 0.5f);
            }

            if (col.gameObject.name.Contains("Player1"))
            {
                int hitAreaValue = col.gameObject.GetComponent<Paddle>().FindHitHalf(yPos);

                if (hitAreaValue == 1)
                    rb2d.AddForce(new Vector2(forceToAddX, forceToAddY));
                else
                    rb2d.AddForce(new Vector2(forceToAddX, -forceToAddY));

                audioManager.Play("LeftPaddleHit");
            }
            else if (col.gameObject.name.Contains("Player2"))
            {
                int hitAreaValue = col.gameObject.GetComponent<Paddle>().FindHitHalf(yPos);

                if (hitAreaValue == 1)
                    rb2d.AddForce(new Vector2(-forceToAddX, forceToAddY));
                else
                    rb2d.AddForce(new Vector2(-forceToAddX, -forceToAddY));

                audioManager.Play("RightPaddleHit");
            }
        }
        else
            audioManager.Play("WallHit");
    }
}
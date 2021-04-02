using UnityEngine;

public class PaddleControls : MonoBehaviour
{
    #region Initialization
    //Booleans
    private bool[] currentlyHolding = new bool[] { false, false };
    private bool[] holdEnded = new bool[] { true, true };
    private bool[] holdingUp = new bool[] { true, true };

    private bool[] playerMoving = new bool[] { false, false };

    //Floats
    private float pastYpos;

    public float player1PaddleMovementSpeed;
    public float player2PaddleMovementSpeed;

    private float originalPlayer1PaddleMovementSpeed;
    private float originalPlayer2PaddleMovementSpeed;

    public float defaultPaddleMovementSpeed;

    public float player1PaddleRotateSpeed;
    public float player2PaddleRotateSpeed;

    public float defaultPaddleRotateSpeed;

    public float yLimit;
    public float yLimitBuffer;

    private float[] startTimes = new float[] { 0, 0 };
    public float[] playerHoldTimes;

    public float velocityCoefficient;

    public float minPaddleVelocity;
    public float maxPaddleVelocity;

    //Controls
    public KeyCode player1Up;
    public KeyCode player1Down;
    public KeyCode player1LeftRotate;
    public KeyCode player1RightRotate;
    public KeyCode player1Charge;

    public KeyCode player2Up;
    public KeyCode player2Down;
    public KeyCode player2LeftRotate;
    public KeyCode player2RightRotate;
    public KeyCode player2Charge;

    //Paddles
    public Transform player1Paddle;
    public Transform player2Paddle;

    private Rigidbody2D player1PaddleRb2D;
    private Rigidbody2D player2PaddleRb2D;

    //Ball
    public Transform ball;

    //Scripts
    private Settings settings;
    private BallPhysics ballPhysics;

    private void Awake()
    {
        settings = FindObjectOfType<Settings>();
        ballPhysics = FindObjectOfType<BallPhysics>();

        player1PaddleRb2D = player1Paddle.GetComponent<Rigidbody2D>();
        player2PaddleRb2D = player2Paddle.GetComponent<Rigidbody2D>();

        originalPlayer1PaddleMovementSpeed = player1PaddleMovementSpeed;
        originalPlayer2PaddleMovementSpeed = player2PaddleMovementSpeed;
    }
    #endregion

    private void Update()
    {
        bool player1 = Input.GetKey(player1Up) || Input.GetKey(player1Down) || Input.GetKey(player1RightRotate) || Input.GetKey(player1LeftRotate);
        bool player2 = Input.GetKey(player2Up) || Input.GetKey(player2Down) || Input.GetKey(player2RightRotate) || Input.GetKey(player2LeftRotate);

        if (BallPhysics.paddleSpeedMultiplier != 0)
        {
            player1PaddleMovementSpeed = originalPlayer1PaddleMovementSpeed * BallPhysics.paddleSpeedMultiplier;
            player2PaddleMovementSpeed = originalPlayer1PaddleMovementSpeed * BallPhysics.paddleSpeedMultiplier;
        }

        //Debug.Log("Player 1: " + player1 + "; Player 2: " + player2);

        #region Movement
        //Player 1
        if (Input.GetKey(player1Up))
            MovePaddle(player1Paddle, true);

        if (Input.GetKey(player1Down))
            MovePaddle(player1Paddle, false);

        if (Input.GetKey(player1RightRotate))
            RotatePaddle(player1Paddle, true);

        if (Input.GetKey(player1LeftRotate))
            RotatePaddle(player1Paddle, false);

        //Player 2
        if (Input.GetKey(player2Up))
            MovePaddle(player2Paddle, true);

        if (Input.GetKey(player2Down))
            MovePaddle(player2Paddle, false);

        if (Input.GetKey(player2RightRotate))
            RotatePaddle(player2Paddle, true);

        if (Input.GetKey(player2LeftRotate))
            RotatePaddle(player2Paddle, false);
        #endregion

        //Velocity effect
        #region Velocity Effect
        if (player1 && !currentlyHolding[0] && !holdEnded[0])
        {
            if (!holdEnded[1])
                EndHold(1);

            EndHold(0);
        }
        else if (player2 && !currentlyHolding[1] && !holdEnded[1])
        {
            if (!holdEnded[0])
                EndHold(0);

            EndHold(1);
        }
        else if (player2 && !currentlyHolding[1] && holdEnded[1])
        {
            if (!player1 && currentlyHolding[0])
                EndHold(0);
        }
        else if (player1 && !currentlyHolding[0] && holdEnded[0])
        {
            if (!player2 && currentlyHolding[1])
                EndHold(1);
        }
        else if (!player1 && !player2)
        {
            if (!holdEnded[0])
                EndHold(0);

            if (!holdEnded[1])
                EndHold(1);
        }
        else if (!player2 && !currentlyHolding[1] && !holdEnded[1])
        {
            if (!holdEnded[1])
                EndHold(1);
        }
        else if (!player1 && !currentlyHolding[0] && !holdEnded[0])
        {
            if (holdEnded[0])
                EndHold(0);
        }

        if (player1 && currentlyHolding[0] && !holdEnded[0])
            playerHoldTimes[0] = Mathf.Round((GetRoundedTime() - startTimes[0]) * 100f) / 100f;
        else
            playerHoldTimes[1] = 0;

        if (player2 && currentlyHolding[1] && !holdEnded[1])
            playerHoldTimes[1] = Mathf.Round((GetRoundedTime() - startTimes[1]) * 100f) / 100f;
        else
            playerHoldTimes[1] = 0;
        #endregion

        //If the paddles are sliding down and they get too close, stop them.
        if (player1PaddleRb2D.velocity.y != 0f)
        {
            if (Mathf.Abs(Mathf.Abs(player1Paddle.position.y) - yLimit) < yLimitBuffer && Mathf.Sign(player1PaddleRb2D.velocity.y) == Mathf.Sign(player1PaddleRb2D.position.y))
                player1PaddleRb2D.velocity = Vector2.zero;
        }

        if (player2PaddleRb2D.velocity.y != 0f)
        {
            if (Mathf.Abs(Mathf.Abs(player2Paddle.position.y) - yLimit) < yLimitBuffer && Mathf.Sign(player2PaddleRb2D.velocity.y) == Mathf.Sign(player2PaddleRb2D.position.y))
                player2PaddleRb2D.velocity = Vector2.zero;
        }
    }

    private void MovePaddle(Transform paddleToMove, bool up)
    {
        //Initialization
        pastYpos = paddleToMove.position.y;

        bool player1 = paddleToMove.name.Contains("Player1");
        float paddleMovementSpeed = 0.0f;

        if (player1)
            paddleMovementSpeed = player1PaddleMovementSpeed;
        else
            paddleMovementSpeed = player2PaddleMovementSpeed;

        //Movement
        if (up)
            paddleToMove.position = new Vector2(paddleToMove.position.x, paddleToMove.position.y + (paddleMovementSpeed * Time.deltaTime));
        else
            paddleToMove.position = new Vector2(paddleToMove.position.x, paddleToMove.position.y - (paddleMovementSpeed * Time.deltaTime));

        //Bounds
        if (/*Distance from y-limit*/ Mathf.Abs(Mathf.Abs(paddleToMove.position.y) - yLimit) < yLimitBuffer)
        {
            float posY;

            if (paddleToMove.position.y > 0)
                posY = yLimit - yLimitBuffer;
            else
                posY = -yLimit + yLimitBuffer;

            paddleToMove.transform.position = new Vector2(paddleToMove.transform.position.x, posY);
        }
    }

    #region Mess
    private void MovePlayer1Paddle(bool up)
    {
        playerMoving[0] = true;

        Transform paddleToMove = player1Paddle;

        //Initialization
        pastYpos = paddleToMove.position.y;

        //Movement
        if (up)
            paddleToMove.position = new Vector2(paddleToMove.position.x, paddleToMove.position.y + (player1PaddleMovementSpeed * Time.deltaTime));
        else
            paddleToMove.position = new Vector2(paddleToMove.position.x, paddleToMove.position.y - (player1PaddleMovementSpeed * Time.deltaTime));

        //Bounds
        if (/*Distance from y-limit*/ Mathf.Abs(Mathf.Abs(paddleToMove.position.y) - yLimit) < yLimitBuffer)
        {
            float posY;

            if (paddleToMove.position.y > 0)
                posY = yLimit - yLimitBuffer;
            else
                posY = -yLimit + yLimitBuffer;

            paddleToMove.transform.position = new Vector2(paddleToMove.transform.position.x, posY);
        }

        playerMoving[0] = false;
    }

    private void MovePlayer2Paddle(bool up)
    {
        playerMoving[1] = true;

        Transform paddleToMove = player2Paddle;

        //Initialization
        pastYpos = paddleToMove.position.y;

        //Movement
        if (up)
            paddleToMove.position = new Vector2(paddleToMove.position.x, paddleToMove.position.y + (player2PaddleMovementSpeed * Time.deltaTime));
        else
            paddleToMove.position = new Vector2(paddleToMove.position.x, paddleToMove.position.y - (player2PaddleMovementSpeed * Time.deltaTime));

        //Bounds
        if (/*Distance from y-limit*/ Mathf.Abs(Mathf.Abs(paddleToMove.position.y) - yLimit) < yLimitBuffer)
        {
            float posY;

            if (paddleToMove.position.y > 0)
                posY = yLimit - yLimitBuffer;
            else
                posY = -yLimit + yLimitBuffer;

            paddleToMove.transform.position = new Vector2(paddleToMove.transform.position.x, posY);
        }

        playerMoving[1] = false;
    }
    #endregion


    private void StartHold(int paddleIndex)
    {
        currentlyHolding[paddleIndex] = true;

        if (startTimes[paddleIndex] == 0)
            startTimes[paddleIndex] = GetRoundedTime();

        //Debug.Log("Player " + (paddleIndex + 1) + " Hold Start Time: " + startTimes[paddleIndex]);

        holdEnded[paddleIndex] = false;
    }

    private void EndHold(int paddleIndex)
    {
        float timeHeld = Mathf.Round((GetRoundedTime() - startTimes[paddleIndex]) * 100f) / 100f;

        startTimes[paddleIndex] = 0;

        currentlyHolding[paddleIndex] = false;
        holdEnded[paddleIndex] = true;

        //Debug.Log("Player " + (paddleIndex + 1) + " Hold End Time: " + GetRoundedTime() + "; Total time held: " + timeHeld + "; Up: " + holdingUp[paddleIndex]);

        if (paddleIndex == 0)
        {
            //Move Player 1's paddle
            if (holdingUp[paddleIndex])
                player1Paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -Mathf.Clamp(Mathf.Pow(velocityCoefficient, timeHeld), minPaddleVelocity, maxPaddleVelocity));
            else
                player1Paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Mathf.Clamp(Mathf.Pow(velocityCoefficient, timeHeld), minPaddleVelocity, maxPaddleVelocity));
        }
        else
        {
            //Move Player 2's paddle
            if (holdingUp[paddleIndex])
                player2Paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -Mathf.Clamp(Mathf.Pow(velocityCoefficient, timeHeld), minPaddleVelocity, maxPaddleVelocity));
            else
                player2Paddle.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, Mathf.Clamp(Mathf.Pow(velocityCoefficient, timeHeld), minPaddleVelocity, maxPaddleVelocity));
        }
    }

    private float GetRoundedTime()
    {
        return Mathf.Round(Time.time * 100f) / 100f;
    }

    private void RotatePaddle(Transform paddleToMove, bool right)
    {
        if (right)
        {
            if (paddleToMove.name.Contains("player1"))
                paddleToMove.rotation = Quaternion.Euler(paddleToMove.eulerAngles.x, paddleToMove.eulerAngles.y, paddleToMove.eulerAngles.z - (player1PaddleRotateSpeed * Time.deltaTime));
            else
                paddleToMove.rotation = Quaternion.Euler(paddleToMove.eulerAngles.x, paddleToMove.eulerAngles.y, paddleToMove.eulerAngles.z - (player2PaddleRotateSpeed * Time.deltaTime));
        }
        else
        {
            if (paddleToMove.name.Contains("player1"))
                paddleToMove.rotation = Quaternion.Euler(paddleToMove.eulerAngles.x, paddleToMove.eulerAngles.y, paddleToMove.eulerAngles.z + (player1PaddleRotateSpeed * Time.deltaTime));
            else
                paddleToMove.rotation = Quaternion.Euler(paddleToMove.eulerAngles.x, paddleToMove.eulerAngles.y, paddleToMove.eulerAngles.z + (player2PaddleRotateSpeed * Time.deltaTime));
        }
    }

    public void SetGeneralPaddleMovementSpeed(float desiredPaddleMovementSpeed)
    {
        player1PaddleMovementSpeed = desiredPaddleMovementSpeed;
        player2PaddleMovementSpeed = desiredPaddleMovementSpeed;
    }

    public void SetGeneralPaddleRotateSpeed(float desiredPaddleRotateSpeed)
    {
        player1PaddleRotateSpeed = desiredPaddleRotateSpeed;
        player2PaddleRotateSpeed = desiredPaddleRotateSpeed;
    }
}
using UnityEngine;
using Mirror;
using System;

public class PaddleControlsMultiplayer : NetworkBehaviour
{
    #region Initialization
    //Booleans
    private bool ready = false;

    //Integers
    private int playerID;

    //Floats
    private float pastYpos;

    public float yLimit;
    public float yLimitBuffer;

    //Movement
    public float defaultMovementSpeed;
    private float movementSpeed;

    //Rotation
    public float defaultRotationSpeed;
    private float rotationSpeed;

    //Controls
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftRotateKey;
    public KeyCode rightRotateKey;

    private Rigidbody2D rb2D;

    //Scripts
    private Settings settings;
    private BallPhysics ballPhysics;

    private MultiplayerGameManager gameManager;

    private void Awake()
    {
        settings = FindObjectOfType<Settings>();
        ballPhysics = FindObjectOfType<BallPhysics>();
        gameManager = FindObjectOfType<MultiplayerGameManager>();

        rb2D = GetComponent<Rigidbody2D>();

        movementSpeed = defaultMovementSpeed;
        rotationSpeed = defaultRotationSpeed;
    }
    #endregion

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerID = MultiplayerPaddleSetup.paddleID;
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (BallPhysics.paddleSpeedMultiplier != 0)
            movementSpeed = defaultMovementSpeed * BallPhysics.paddleSpeedMultiplier;

        #region Movement
        //Player 1
        if (Input.GetKey(upKey))
            MovePaddle(true);

        if (Input.GetKey(downKey))
            MovePaddle(false);

        if (Input.GetKey(rightRotateKey))
            RotatePaddle(true);

        if (Input.GetKey(leftRotateKey))
            RotatePaddle(false);
        #endregion

        //If the paddles are sliding down and they get too close, stop them.
        if (rb2D.velocity.y != 0f)
        {
            if (Mathf.Abs(Mathf.Abs(transform.position.y) - yLimit) < yLimitBuffer && Mathf.Sign(rb2D.velocity.y) == Mathf.Sign(rb2D.position.y))
                rb2D.velocity = Vector2.zero;
        }
    }

    private void MovePaddle(bool up)
    {
        if (!ready) UpdatePlayerStatus(true);
        ready = true;

        //Initialization
        pastYpos = transform.position.y;

        //Movement
        if (up)
            transform.position = new Vector2(transform.position.x, transform.position.y + (movementSpeed * Time.deltaTime));
        else
            transform.position = new Vector2(transform.position.x, transform.position.y - (movementSpeed * Time.deltaTime));

        //Bounds
        if (/*Distance from y-limit*/ Mathf.Abs(Mathf.Abs(transform.position.y) - yLimit) < yLimitBuffer)
        {
            float posY;

            if (transform.position.y > 0)
                posY = yLimit - yLimitBuffer;
            else
                posY = -yLimit + yLimitBuffer;

            transform.transform.position = new Vector2(transform.transform.position.x, posY);
        }
    }

    private void RotatePaddle(bool right)
    {
        if (!ready) UpdatePlayerStatus(true);
        ready = true;

        if (right)
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - (rotationSpeed * Time.deltaTime));
        else
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + (rotationSpeed * Time.deltaTime));
    }

    ///<summary>Triggers when a player is ready. An int (player ID, 1 or 2) is passed as an argument.</summary>
    public static event Action<int> OnPlayerReady;

    [Command]
    private void UpdatePlayerStatus(bool ready)
    {
        FindObjectOfType<UIAnimationManager>().TransitionKeyUIColor(true);

        OnPlayerReady?.Invoke(playerID);

        Debug.Log("Player " + playerID + " is " + (!ready ? "not" : "") + "ready.");
    }
}
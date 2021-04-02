using UnityEngine;

public class ScalePaddles : MonoBehaviour
{
    //Booleans
    public bool multiplayer;

    //Floats
    public float widthDivisor;

    //GameObjects
    public GameObject paddleLeft;
    public GameObject paddleRight;

    private void Start()
    {
        UpdatePaddlePositions(true);

        MultiplayerNetworkManager.OnConnected += UpdatePaddlePositions;
        MultiplayerNetworkManager.OnPlayerChange += UpdatePaddlePositions;
    }

    private void UpdatePaddlePositions() => UpdatePaddlePositions(true);
    private void UpdatePaddlePositions(bool bothPlayers)
    {
        if (multiplayer)
        {
            var paddles = GameObject.FindGameObjectsWithTag("Paddle");

            if (paddles.Length > 0)
                paddleLeft = paddles[0];

            if (paddles.Length > 1)
                paddleRight = paddles[1];
        }

        Vector3 leftPaddlePosValues = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / widthDivisor, Screen.height, 0f));
        Vector3 rightPaddlePosValues = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - (Screen.width / widthDivisor), Screen.height, 0f));

        if (paddleLeft != null)
            paddleLeft.transform.position = new Vector2(leftPaddlePosValues.x, paddleLeft.transform.position.y);

        if (paddleRight != null)
            paddleRight.transform.position = new Vector2(rightPaddlePosValues.x, paddleRight.transform.position.y);
    }
}
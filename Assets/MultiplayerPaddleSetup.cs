using Mirror;
using UnityEngine;

public class MultiplayerPaddleSetup : NetworkBehaviour
{
    public static int paddleID;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        var paddles = GameObject.FindGameObjectsWithTag("Paddle");

        name = "Paddle " + paddles.Length;

        paddleID = paddles.Length;

        FindObjectOfType<MultiplayerGameManager>().playerID = paddleID;
    }
}

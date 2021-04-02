using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class MultiplayerNetworkManager : NetworkManager
{
    public Transform leftPaddleSpawnPos;
    public Transform rightPaddleSpawnPos;

    private GameObject ball;

    //Events

    /// <summary> Triggers whenever a player joins or leaves. Returns true if the room is now full and returns false when the room is not. </summary>
    public static event Action<bool> OnPlayerChange;


    public static event Action OnConnected;

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //Decide where to spawn the player dependent on if they are the first or the second to connect.
        Transform start = numPlayers == 0 ? leftPaddleSpawnPos : rightPaddleSpawnPos;

        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);

        //Spawn in the ball if both players are connected.
        if (numPlayers == 2)
        {
            OnPlayerChange?.Invoke(true);

            ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
            NetworkServer.Spawn(ball);
        }
        else
            OnPlayerChange?.Invoke(false);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        OnPlayerChange?.Invoke(false);

        //Destroy the ball if there is only one player connected.
        if (ball != null)
            NetworkServer.Destroy(ball);

        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        OnConnected?.Invoke();
    }
}

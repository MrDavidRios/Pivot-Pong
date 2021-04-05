using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectButton : MonoBehaviour
{
    void Start()
    {
        var networkManager = FindObjectOfType<MultiplayerNetworkManager>();

        GetComponent<Button>().onClick.AddListener(() =>
            {
                networkManager.StopServer();
                networkManager.StopClient();
            });
    }
}
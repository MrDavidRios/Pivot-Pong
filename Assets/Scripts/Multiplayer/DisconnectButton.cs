using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectButton : MonoBehaviour
{
    private void Start()
    {
        var networkManager = FindObjectOfType<MultiplayerNetworkManager>();

        GetComponent<Button>().onClick.AddListener(() =>
            {
                networkManager.StopServer();
                networkManager.StopClient();
            });
    }
}
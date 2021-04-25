using System.Collections;
using UnityEngine;

public class ConnectToAddressUI : MonoBehaviour
{
    public GameObject connectionUI;
    public GameObject connectingText;

    public GameObject endgameScreen;

    public GameObject backArrow;

    public TMPro.TMP_InputField addressInput;

    [SerializeField] private MultiplayerNetworkManager networkManager;

    private void Awake()
    {
        if (GameSetup.hosting)
        {
            Debug.Log("Hosting server.");

            //Automatically start host + server config
            networkManager.StartHost();
        }
        else
        {
            Debug.Log("Client.");

            //Open connection UI
            connectionUI.SetActive(true);

            connectingText.SetActive(false);
            backArrow.SetActive(false);
        }

        MultiplayerNetworkManager.OnConnected += CloseConnectionUI;
        MultiplayerNetworkManager.OnDisconnected += OpenConnectionUI;
    }

    //Called when the 'Connect' button is pressed in the connection UI.
    public void ConnectToAddress()
    {
        connectionUI.SetActive(false);
        connectingText.SetActive(true);

        if (networkManager == null)
            networkManager = FindObjectOfType<MultiplayerNetworkManager>();

        networkManager.StartClient();
        networkManager.networkAddress = addressInput.text;

        Debug.Log("Connecting to " + networkManager.networkAddress + "...");
    }

    private void OpenConnectionUI()
    {
        if (connectionUI == null)
            return;

        if (endgameScreen.activeInHierarchy)
            StartCoroutine(WaitUntilEndgameScreenClosed());
        else
        {
            connectionUI.SetActive(true);
            connectingText.SetActive(false);
        }
    }

    private void CloseConnectionUI()
    {
        if (connectionUI == null)
            return;

        connectionUI.SetActive(false);
        connectingText.SetActive(false);
        backArrow.SetActive(false);
    }

    private IEnumerator WaitUntilEndgameScreenClosed()
    {
        yield return new WaitUntil(() => !endgameScreen.activeInHierarchy);

        connectionUI.SetActive(true);
        connectingText.SetActive(false);
    }
}

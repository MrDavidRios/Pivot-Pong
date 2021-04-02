using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DisableTextWhenRoomFull : MonoBehaviour
{
    public bool enableWhenRoomNotFull;

    void OnEnable()
    {
        MultiplayerNetworkManager.OnPlayerChange += ActivateText;

        //FIND WAY TO GET PLAYER COUNT 
        ActivateText(NetworkServer.connections.Count == 2);
    }

    private void ActivateText(bool roomFull)
    {
        if (!enableWhenRoomNotFull && !roomFull) return;
        GetComponent<TMPro.TMP_Text>().enabled = !roomFull;
    }
}

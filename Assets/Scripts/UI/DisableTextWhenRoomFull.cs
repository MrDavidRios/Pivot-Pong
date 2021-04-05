using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DisableTextWhenRoomFull : NetworkBehaviour
{
    public bool enableWhenRoomNotFull;

    public override void OnStartClient()
    {
        MultiplayerNetworkManager.OnPlayerChange += ActivateText;

        ActivateText(NetworkServer.connections.Count == 2);
    }

    private void ActivateText(bool roomFull) => GetComponent<TMPro.TMP_Text>().enabled = !roomFull;
}

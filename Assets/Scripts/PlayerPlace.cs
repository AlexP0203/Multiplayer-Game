using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Unity.Netcode;

public class PlayerPlace : NetworkBehaviour
{
    public NetworkVariable<int> Place = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> playerNumber = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public void Start()
    {
        Place.Value = 0;
        playerNumber.Value = 0;
    }

    public void ChangePlace()
    {
        Place.Value += 1;
    }

    public void ChangePlayerNumber()
    {
        playerNumber.Value += 1;
    }

    [ServerRpc(RequireOwnership = false)]
    public void displayPlayerPlaceServerRpc(string n)
    {
        switch (Place.Value)
        {
            case 1:
                SendMessageToClients(n + " Wins");
                break;
            case 2:
                SendMessageToClients(n + " 2nd Place");
                break;
            case 3:
                SendMessageToClients(n + " 3rd Place");
                break;
            case 4:
                SendMessageToClients(n + " Last Place");
                break;
        }
    }

    [ClientRpc]
    void PrintToAllClientsClientRpc(string message)
    {
        Debug.Log(message);
    }

    // Call this function on the server to send the message to all clients

    public void SendMessageToClients(string message)
    {
        if (IsServer)
        {
            PrintToAllClientsClientRpc(message);
        }
    }

}

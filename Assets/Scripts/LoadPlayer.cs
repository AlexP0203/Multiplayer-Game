using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class LoadPlayer : NetworkBehaviour
{
    [SerializeField] Transform[] spawnPos;
    [SerializeField] Object[] playerPrefabs;
    private NetworkVariable<int> playerNum = new NetworkVariable<int>();
    private const int initialValue = 0;

    private void Awake()
    {
        SelectedCharacter.characterSpawn += SpawnPlayer;
        if (IsServer)
        {
            playerNum.Value = initialValue;
        }
    }

    public void SpawnPlayer()
    {
        if (playerNum.Value < 4)
        {
            Object go = Instantiate(playerPrefabs[SelectedCharacter.character], spawnPos[playerNum.Value++].position, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}

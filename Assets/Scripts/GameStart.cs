using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject spawnedPlayer;
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            spawnedPlayer = col.gameObject;
        }
    }
}

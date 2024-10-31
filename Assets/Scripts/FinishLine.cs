using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    string playerName;
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "Player")
        {
            //playerName = col.gameObject.name;
            FindObjectOfType<PlayerPlace>().ChangePlace();
            col.gameObject.GetComponent<CharacterControls>().stopPlayer();
            //col.gameObject.GetComponent<CharacterControls>().displayPlayerPlace(FindObjectOfType<PlayerPlace>().Place.Value);
            FindObjectOfType<PlayerPlace>().displayPlayerPlaceServerRpc(col.gameObject.name);
        }
    }
}

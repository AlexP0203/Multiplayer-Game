using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Events;

public class SwitchScene : NetworkBehaviour
{
    Button button;
    [SerializeField] Vector3[] spawnPos;
    [SerializeField] Object[] playerPrefabs;
    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SwitchingScene);
    }

    void SwitchingScene()
    {
        if (SelectedCharacter.character != -1)
        {
            var status = NetworkManager.SceneManager.LoadScene("Level01", LoadSceneMode.Single);
            if (status == SceneEventProgressStatus.Started)
            {
                NetworkManager.Singleton.SceneManager.OnSceneEvent += SelectedCharacter.OnSceneEvent;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public static class SelectedCharacter 
{
    public static int character = -1;
    public static UnityAction characterSpawn;
    public static int charIndex = 0;

    public static void OnSceneEvent(SceneEvent scene)
    {
        if (scene.SceneEventType == SceneEventType.LoadComplete)
        {
            characterSpawn.Invoke();
        }
    }

}

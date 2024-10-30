using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    CharacterControls characterControls;
    BoxCollider boxCollider;
    [SerializeField] AnimationClip clip;
    AnimationEvent aniEvent = new AnimationEvent();
    private void OnEnable()
    {
        characterControls = GetComponentInParent<CharacterControls>();
        boxCollider = GetComponentInChildren<BoxCollider>();
        characterControls.punch += PunchHitBox;
        aniEvent.time = 1.7f;
        aniEvent.functionName = "PunchHitBoxEnd";
        clip.AddEvent(aniEvent);
    }

    public void PunchHitBox()
    {
        boxCollider.enabled = true;
    }
    public void PunchHitBoxEnd()
    {
        boxCollider.enabled = false;
    }
}

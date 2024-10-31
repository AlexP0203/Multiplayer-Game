using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPeedBooster : MonoBehaviour
{
    [SerializeField] CharacterControls characterControls;
    [SerializeField] float speedBoost;
    [SerializeField] float boostDuration;

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<CharacterControls>().SetSpeed(30.0f);
    }
}

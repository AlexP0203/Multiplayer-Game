using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    Animator anim;
    CharacterControls charControls;
    [SerializeField] AnimationClip jumpClip;

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        CharacterControls charControls = GetComponentInParent<CharacterControls>();
        charControls.moving += PlayerMoving;
        charControls.stopped += PlayerStopped;
        charControls.running += PlayerRunning;
        charControls.jumping += PlayerJumping;
        charControls.falling += PlayerFalling;
    }

    void PlayerMoving(bool moving)
    {
        anim.SetBool("jogging", moving);
    }

    void PlayerStopped(Rigidbody rb)
    {
        anim.SetBool("jogging", false);
        anim.SetBool("running", false);
    }

    void PlayerRunning(float[] speeds)
    {
        if (speeds[0] > speeds[1])
        {
           anim.SetBool("running", true);
        }
        else if (speeds[0] < speeds[2])
        {
            anim.SetBool("running", false);
        }
    }

    void PlayerJumping(bool isGrounded)
    {
        anim.SetBool("jumping", isGrounded);
        anim.SetBool("jumped", !isGrounded);
        anim.SetBool("falling", false);
    }

    public void PlayerJumped()
    {
        anim.SetBool("jumped", true);
    }

    public void PlayerFalling(float downwardSpeed)
    {
        if (downwardSpeed < -0.5f)
        {
            anim.SetBool("falling", true);
        }
        else
        {
            anim.SetBool("falling", false);
        }
    }
}

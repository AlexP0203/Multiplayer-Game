using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        collision.transform.SetParent(transform);
    }

    void OnCollisionExit(Collision collision)
    {
        collision.transform.SetParent(null);
    }
}



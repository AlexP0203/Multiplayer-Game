using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.Rotate(0f, speed * Time.deltaTime / 0.01f, 0f, Space.Self);
    }
}
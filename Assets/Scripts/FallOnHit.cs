using System;
using UnityEngine;

public class FallOnHit : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
        enabled = false;
    }
}

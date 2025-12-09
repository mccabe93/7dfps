using System;
using UnityEngine;

public class TrampolineBase : MonoBehaviour
{
    public float UpwardForce = 300f;

    private void OnTriggerEnter(Collider other)
    {
        var tbase = other.gameObject.GetComponent<TrampolineTile>();
        var rb = tbase.RigidBody;
        if (rb.linearVelocity.magnitude < UpwardForce)
        {
            rb.linearVelocity = Vector3.up * UpwardForce;
        }
    }
}

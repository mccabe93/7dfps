using System;
using UnityEngine;

public class TrampolineBase : MonoBehaviour
{
    public float UpwardForce = 300f;

    private void OnTriggerEnter(Collider other)
    {
        var tbase = other.gameObject.GetComponent<TrampolineTile>();
        var rb = tbase.RigidBody;
        foreach (var collider in tbase.Floor.Colliders)
        {
            collider.rigidbody.AddForceAtPosition(
                Vector3.up * UpwardForce * Math.Abs(collider.rigidbody.linearVelocity.y),
                rb.transform.position + Vector3.down,
                ForceMode.Force
            );
        }
    }
}

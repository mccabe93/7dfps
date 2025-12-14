using System;
using UnityEngine;

public class TrampolineBase : MonoBehaviour
{
    public float UpwardForce = 300f;
    public Collider Tile;

    private TrampolineTile _trampolineTile;

    private void Start()
    {
        _trampolineTile = Tile.GetComponentInChildren<TrampolineTile>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == Tile)
        {
            var rb = _trampolineTile.RigidBody;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * UpwardForce, ForceMode.Impulse);
            foreach (var child in _trampolineTile.Floor.CollidingObjects)
            {
                if (child != rb)
                {
                    child.angularVelocity = Vector3.zero;
                    child.linearVelocity = Vector3.zero;
                    child.AddForce(Vector3.up * UpwardForce, ForceMode.Impulse);
                }
            }
        }
    }
}

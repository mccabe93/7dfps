using UnityEngine;

public class TrampolineLimit : MonoBehaviour
{
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
            _trampolineTile.RigidBody.angularVelocity = Vector3.zero;
            _trampolineTile.RigidBody.linearVelocity = Vector3.zero;
            _trampolineTile.RigidBody.useGravity = false;
        }
    }
}

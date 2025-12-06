using UnityEngine;

public class TrampolineLimit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var tbase = other.gameObject.GetComponent<TrampolineTile>();
        var rb = tbase.RigidBody;
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
    }
}

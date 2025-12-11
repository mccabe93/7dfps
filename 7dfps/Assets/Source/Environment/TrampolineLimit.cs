using UnityEngine;

public class TrampolineLimit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var tbase = other.gameObject.GetComponent<TrampolineTile>();
        if (tbase != null)
        {
            var rb = tbase.RigidBody;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}

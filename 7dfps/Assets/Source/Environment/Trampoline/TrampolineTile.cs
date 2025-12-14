using System.Collections.Generic;
using UnityEngine;

public class TrampolineTile : MonoBehaviour
{
    public Rigidbody RigidBody;
    public FloorBase Floor;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null && collision.gameObject.tag != "Ground")
        {
            RigidBody.useGravity = true;
        }
    }
}

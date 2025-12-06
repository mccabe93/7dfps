using System.Collections.Generic;
using UnityEngine;

public class FloorBase : MonoBehaviour
{
    public HashSet<Collision> Colliders = new HashSet<Collision>();

    private void OnCollisionEnter(Collision collision)
    {
        if (!Colliders.Contains(collision))
        {
            Colliders.Add(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Colliders.Contains(collision))
        {
            Colliders.Remove(collision);
        }
    }
}

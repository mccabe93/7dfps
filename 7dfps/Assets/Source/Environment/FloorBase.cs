using System.Collections.Generic;
using UnityEngine;

public class FloorBase : MonoBehaviour
{
    public Transform Limit;
    public Transform Floor;
    public HashSet<Rigidbody> CollidingObjects = new HashSet<Rigidbody>();

    public void Update()
    {
        if (transform.position.y > Limit.position.y)
        {
            transform.position = Limit.position;
        }
        else if (transform.position.y < Floor.position.y)
        {
            transform.position = Floor.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null && collision.gameObject.tag != "Ground")
        {
            CollidingObjects.Add(collision.rigidbody);
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyActor>().OnDeath += () =>
                {
                    CollidingObjects.Remove(collision.rigidbody);
                };
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.rigidbody != null && collision.gameObject.tag != "Ground")
        {
            CollidingObjects.Remove(collision.rigidbody);
        }
    }
}

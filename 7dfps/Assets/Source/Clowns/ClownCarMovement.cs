using UnityEngine;
using UnityEngine.AI;

public class ClownCarMovement : MonoBehaviour
{
    public EnemyActor Actor;

    public Rayfinder Rayfinder;
    public Rigidbody Rigidbody;

    private GroundedChecker _groundedChecker;

    private void Start()
    {
        _groundedChecker = gameObject.AddComponent<GroundedChecker>();
        Rayfinder.Source = this.GetComponentInParent<Transform>();
        Rayfinder.Destination = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Actor.IsAlive)
        {
            Rayfinder.Move(_groundedChecker.IsGrounded);
        }
    }
}

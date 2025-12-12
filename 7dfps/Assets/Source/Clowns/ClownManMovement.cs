using UnityEditor.SceneManagement;
using UnityEngine;

public class ClownMan : MonoBehaviour
{
    public EnemyActor Actor;

    public GameObject Ragdoll;
    public Rayfinder Rayfinder;
    public Rigidbody Rigidbody;
    private GroundedChecker _groundedChecker;

    private bool _isAttacking;

    private void Start()
    {
        _groundedChecker = gameObject.AddComponent<GroundedChecker>();
        Rayfinder.Source = this.GetComponentInParent<Transform>();
        Rayfinder.Destination = GameObject.FindGameObjectWithTag("Player").transform;
        Actor.Animator.StartPlayback();
    }

    private void Update()
    {
        if (Actor.IsAlive)
        {
            if (_isAttacking)
            {
                if (!Actor.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                {
                    _isAttacking = false;
                }
                else
                {
                    return;
                }
            }
            MovementResult movementResult = Rayfinder.Move(_groundedChecker.IsGrounded);
            switch (movementResult)
            {
                case MovementResult.Rotating:
                case MovementResult.Moved:
                    if (!Actor.Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                    {
                        Actor.Animator.Play("Walk", 0);
                    }
                    break;
                case MovementResult.ReachedDestination:
                    if (!Actor.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
                    {
                        _isAttacking = true;
                        Actor.Animator.Play("Attack1", 0);
                    }
                    break;
            }
        }
        else
        {
            if (!Actor.Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                Actor.Animator.Play("Death");
            }
        }
        /*
        else if (!Ragdoll.activeSelf)
        {
            Destroy(GetComponent<BoxCollider>());
            Destroy(Rigidbody);
            //Ragdoll.SetActive(true);
        }
        */
    }
}

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ClownCar : MonoBehaviour
{
    public EnemyActor Actor;

    public Transform[] BulletSpawnPoints;
    public Bullet ClownCarBullet;
    public MuzzleFlashPlayer MuzzleFlashPlayer;

    public float AttackInterval = 2f;
    public float AttackDamage = 5f;

    public WaypointMover WaypointMover;

    private GroundedChecker _groundedChecker;
    private Transform _player;

    private bool _isAttacking = false;

    private void Start()
    {
        _groundedChecker = gameObject.AddComponent<GroundedChecker>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Actor.IsAlive)
        {
            WaypointMover.Move();
            if (!_isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;
        foreach (var spawnPoint in BulletSpawnPoints)
        {
            // Direction from spawn point toward the player (aim at player center mass)
            Vector3 targetPosition = _player.position + Vector3.up;
            Vector3 directionToPlayer = (targetPosition - spawnPoint.position).normalized;

            Ray shootPlayerRay = new Ray(spawnPoint.position, directionToPlayer);
            Debug.DrawRay(shootPlayerRay.origin, shootPlayerRay.direction * 100f, Color.red, 1f);

            if (Physics.Raycast(shootPlayerRay, out RaycastHit hitInfo, 100f))
            {
                if (hitInfo.collider.gameObject != _player.gameObject)
                {
                    continue;
                }
                MuzzleFlashPlayer.StartFX();
                Bullet bullet = Instantiate(
                    ClownCarBullet,
                    spawnPoint.position,
                    Quaternion.LookRotation(directionToPlayer)
                );
                // Set direction in world space — Bullet applies force along this vector
                bullet.Direction = directionToPlayer;
                bullet.Damage = AttackDamage;
            }
        }
        yield return new WaitForSeconds(AttackInterval);
        _isAttacking = false;
    }
}

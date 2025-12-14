using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ClownCar : MonoBehaviour
{
    public EnemyActor Actor;

    public GameObject[] SpawnableEnemies;
    public Bullet ClownCarBullet;
    public MuzzleFlashPlayer MuzzleFlashPlayer;

    public bool SpawnsEnemies = true;
    public Transform SpawnLocation;
    public float SpawnInterval = 10f;
    public bool SpawnWithAmmo = false;

    public float AttackInterval = 2f;
    public float AttackDamage = 5f;

    public float CollideDamage = 40f;
    public float CollideForce = 150f;

    public AudioSource SpawnClownSound;
    public AudioSource DamagedSound;
    public AudioSource ShootSound;

    public WaypointMover WaypointMover;

    private GroundedChecker _groundedChecker;
    private Transform _player;

    private Transform _muzzle;

    private bool _isAttacking = false;
    private bool _isSpawning = false;

    private void Start()
    {
        _groundedChecker = gameObject.AddComponent<GroundedChecker>();
        _player = GameObject.FindGameObjectWithTag("PlayerBody").transform;
        _muzzle = GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.tag == "WeaponMuzzle")
            ?.transform;
        Actor.OnHit += (damage) =>
        {
            DamagedSound.Play();
            int colorToUse = Random.Range(0, 3);
            switch (colorToUse)
            {
                case 0:
                    Actor.FlashVfx.Color = Color.red;
                    break;
                case 1:
                    Actor.FlashVfx.Color = Color.green;
                    break;
                case 2:
                    Actor.FlashVfx.Color = Color.blue;
                    break;
            }
        };
    }

    private void Update()
    {
        if (Actor.IsAlive)
        {
            WaypointMover.Move();
            if (!_isAttacking)
            {
                _isAttacking = true;
                StartCoroutine(Attack());
            }
            if (SpawnsEnemies && !_isSpawning)
            {
                _isSpawning = true;
                StartCoroutine(SpawnEnemies());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Actor.IsAlive && collision.gameObject.tag == "PlayerBody")
        {
            var rb = collision.gameObject.GetComponentInChildren<Rigidbody>();
            rb.AddForce(Vector3.up * CollideForce, ForceMode.VelocityChange);
            var playerHealth = collision.gameObject.GetComponentInChildren<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(CollideDamage);
            }
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(AttackInterval);
        if (!Actor.IsAlive)
        {
            _isAttacking = false;
            yield break;
        }
        // Direction from spawn point toward the player (aim at player center mass)
        Vector3 targetPosition = _player.position + Vector3.up;
        Vector3 directionToPlayer = (targetPosition - _muzzle.position).normalized;

        Ray shootPlayerRay = new Ray(_muzzle.position, directionToPlayer);
        Debug.DrawRay(shootPlayerRay.origin, shootPlayerRay.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(shootPlayerRay, out RaycastHit hitInfo, 100f))
        {
            if (hitInfo.collider.gameObject != _player.gameObject)
            {
                yield return null;
            }
            ShootSound.Play();
            MuzzleFlashPlayer.StartFX();
            Bullet bullet = Instantiate(
                ClownCarBullet,
                _muzzle.position,
                Quaternion.LookRotation(directionToPlayer)
            );
            // Set direction in world space — Bullet applies force along this vector
            bullet.Direction = directionToPlayer;
            bullet.Damage = AttackDamage;
        }

        _isAttacking = false;
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(SpawnInterval);
        SpawnClownSound.Play();
        var enemyPrefab = SpawnableEnemies[Random.Range(0, SpawnableEnemies.Length)];
        var enemy = Instantiate(enemyPrefab, SpawnLocation.position, Quaternion.identity);
        if (!SpawnWithAmmo)
        {
            var enemyActor = enemy.GetComponentInChildren<ClownMan>();
            enemyActor.Ammo = 0;
        }
        _isSpawning = false;
    }
}

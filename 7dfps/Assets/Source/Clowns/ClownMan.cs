using System.Collections;
using UnityEngine;

public class ClownMan : MonoBehaviour
{
    public EnemyActor Actor;
    public Rayfinder Rayfinder;
    public Rigidbody Rigidbody;
    public Animation Animation;

    public AudioSource DamagedSound;
    public AudioSource ShootSound;
    public AudioSource HitPlayerSound;

    public bool ImmediateMeleeDamage = false;
    public float MeleeAttackTime = 1.0f;
    public float MeleeAttackDamage = 10f;
    public int Ammo = 6;
    public float RangedAttackTime = 1.0f;
    public float RangedAttackDamage = 5.0f;

    public Transform Muzzle;
    public MuzzleFlashPlayer MuzzleFlashPlayer;
    public Bullet ClownGunBullet;
    private GroundedChecker _groundedChecker;

    private Transform _playerPosition;
    private PlayerHealth _playerHealthManager;
    private bool _isAttacking;
    private bool _hasDied = false;

    private void Start()
    {
        _groundedChecker = gameObject.AddComponent<GroundedChecker>();
        _playerHealthManager = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponentInChildren<PlayerHealth>();
        Rayfinder.Source = this.GetComponentInParent<Transform>();
        Rayfinder.Destination = GameObject.FindGameObjectWithTag("PlayerBody").transform;
        _playerPosition = Rayfinder.Destination;
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

    private void Reset()
    {
        Actor = GetComponent<EnemyActor>();
        Rayfinder = GetComponent<Rayfinder>();
        Rigidbody = GetComponent<Rigidbody>();
        Animation = GetComponent<Animation>();
        Muzzle = GetComponentInChildren<Transform>().Find("WeaponMuzzle").transform;
    }

    private void Update()
    {
        if (Actor.IsAlive)
        {
            if (_isAttacking)
            {
                if (!Animation.isPlaying)
                {
                    _isAttacking = false;
                }
                return;
            }
            if (Ammo > 0)
            {
                Vector3 targetPosition = _playerPosition.position + Vector3.up;
                Vector3 directionToPlayer = (targetPosition - Muzzle.position).normalized;
                if (CanShootPlayer(directionToPlayer))
                {
                    _isAttacking = true;
                    Animation.Play("Shoot");
                    StartCoroutine(RangedAttack());
                    return;
                }
            }
            MovementResult movementResult = Rayfinder.Move(_groundedChecker.IsGrounded);
            switch (movementResult)
            {
                case MovementResult.Rotating:
                case MovementResult.Moved:
                    if (!Animation.isPlaying)
                    {
                        Animation.Play("Walk");
                    }
                    break;
                case MovementResult.ReachedDestination:
                    _isAttacking = true;
                    Animation.Play("Melee");
                    StartCoroutine(MeleeAttack());
                    break;
            }
        }
        else
        {
            if (!_hasDied)
            {
                _hasDied = true;
                Animation.Play("Death");
            }
        }
    }

    private IEnumerator MeleeAttack()
    {
        if (ImmediateMeleeDamage)
        {
            _playerHealthManager.TakeDamage(MeleeAttackDamage);
            HitPlayerSound.Play();
        }
        yield return new WaitForSeconds(MeleeAttackTime);
        if (
            Actor.IsAlive
            && !_hasDied
            // Ensure player is still in attack range
            && Rayfinder.Move(_groundedChecker.IsGrounded) == MovementResult.ReachedDestination
        )
        {
            _playerHealthManager.TakeDamage(MeleeAttackDamage);
        }
    }

    private IEnumerator RangedAttack()
    {
        yield return new WaitForSeconds(RangedAttackTime);
        ShootSound.Play();
        _isAttacking = true;
        Vector3 targetPosition = _playerPosition.position + Vector3.up;
        Vector3 directionToPlayer = (targetPosition - Muzzle.position).normalized;
        if (CanShootPlayer(directionToPlayer))
        {
            MuzzleFlashPlayer.StartFX();
            Bullet bullet = Instantiate(
                ClownGunBullet,
                Muzzle.position,
                Quaternion.LookRotation(directionToPlayer)
            );
            bullet.Direction = directionToPlayer;
            bullet.Damage = RangedAttackDamage;
            Ammo--;
        }
    }

    private bool CanShootPlayer(Vector3 directionToPlayer)
    {
        Ray shootPlayerRay = new Ray(Muzzle.position, directionToPlayer);
        Debug.DrawRay(shootPlayerRay.origin, shootPlayerRay.direction * 100f, Color.red, 1f);
        if (Physics.Raycast(shootPlayerRay, out RaycastHit hitInfo, 100f))
        {
            if (hitInfo.collider.gameObject.tag != "PlayerBody")
            {
                return false;
            }
            return true;
        }
        return false;
    }
}

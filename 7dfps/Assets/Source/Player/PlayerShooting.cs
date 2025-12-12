using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerShooting : MonoBehaviour
{
    public GameObject Player;
    public GameObject Bullet;
    public Transform WeaponTransform;
    public GameObject MuzzleFlash;

    private Animator _muzzleFlash;
    private SpriteRenderer _muzzleFlashRenderer;

    public int AmmoCount = 10;
    public int MaxAmmoCount = 10;
    public int ClipAmmo = 5;
    public int ClipCapacity = 5;

    public float ReloadTime = 2.0f;
    public float FireRate = 0.5f;

    public InputActionReference ShootAction;

    public InputActionReference ReloadAction;

    private bool _isShooting = false;
    private bool _isReloading = false;
    private bool _canAct => !_isShooting && !_isReloading;

    private Transform WeaponMuzzle;
    private Transform Camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WeaponMuzzle = GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.tag == "WeaponMuzzle")
            ?.transform;
        Camera = Player
            .GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.tag == "MainCamera")
            ?.transform;
        _muzzleFlash = MuzzleFlash.GetComponent<Animator>();
        _muzzleFlashRenderer = MuzzleFlash.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            _muzzleFlashRenderer.enabled
            && !_muzzleFlash.GetCurrentAnimatorStateInfo(0).IsName("muzzle-flash")
        )
        {
            _muzzleFlashRenderer.enabled = false;
        }

        if (_canAct && ShootAction.action.triggered)
        {
            StartCoroutine("Shoot");
        }

        if (_canAct && ReloadAction.action.triggered)
        {
            StartCoroutine("Reload");
        }

        float rotationX = Camera.rotation.eulerAngles.x;
        if (rotationX > 180f)
        {
            rotationX -= 360f;
        }
        float adjustment = rotationX > 0 ? -1.5f * (rotationX / 180f) : 1.5f * (rotationX / 180f);
        WeaponTransform.localPosition = new Vector3(
            WeaponTransform.localPosition.x,
            WeaponTransform.localPosition.y,
            1.5f + adjustment
        );
    }

    private IEnumerator Shoot()
    {
        _isShooting = true;
        Fire();
        yield return new WaitForSeconds(FireRate);
        _isShooting = false;
    }

    private IEnumerator Reload()
    {
        _isReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        if (_isReloading)
        {
            if (AmmoCount - ClipCapacity < 0)
            {
                ClipAmmo = AmmoCount;
                AmmoCount = 0;
            }
            else
            {
                ClipAmmo = ClipCapacity;
                AmmoCount -= ClipCapacity;
            }
        }
        _isReloading = false;
    }

    public void Fire()
    {
        if (ClipAmmo > 0 && !_isReloading)
        {
            _isReloading = false;
            ClipAmmo -= 1;
            _muzzleFlashRenderer.enabled = true;
            _muzzleFlash.Play("muzzle-flash", -1, 0f);
            GameObject bullet = Instantiate(
                Bullet,
                WeaponMuzzle.position,
                Camera.transform.rotation
            );
            var bulletProperties = bullet.GetComponent<Bullet>();
            bulletProperties.Direction = Camera.transform.forward;
        }
    }
}

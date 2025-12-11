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
    }

    // Update is called once per frame
    void Update()
    {
        if (_canAct && ShootAction.action.triggered)
        {
            StartCoroutine("Shoot");
        }

        if (_canAct && ReloadAction.action.triggered)
        {
            StartCoroutine("Reload");
        }

        transform.position =
            Camera.transform.position
            + Camera.transform.forward * 1.5f
            + Camera.transform.right * 0.75f
            + Camera.transform.up * -0.5f;
        transform.LookAt(Camera.transform.position - Camera.transform.forward * 10f);
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
            GameObject bullet = Instantiate(
                Bullet,
                WeaponMuzzle.position,
                Camera.transform.rotation
            );
            bullet.GetComponent<Rigidbody>().linearVelocity = Camera.transform.forward * 10f;
        }
    }
}

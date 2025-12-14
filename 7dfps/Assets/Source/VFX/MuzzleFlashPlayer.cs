using System.Linq;
using UnityEngine;

public class MuzzleFlashPlayer : MonoBehaviour
{
    public GameObject MuzzleFlash;

    private Animator _muzzleFlash;
    private Transform _weaponMuzzle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _weaponMuzzle = GetComponentsInChildren<Transform>()
            .FirstOrDefault(t => t.tag == "WeaponMuzzle")
            ?.transform;
        _muzzleFlash = MuzzleFlash.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() { }

    public void StartFX()
    {
        _muzzleFlash.Play("muzzle-flash", -1, 0f);
    }
}

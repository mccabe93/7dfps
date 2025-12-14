using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public List<Image> AmmoIcons;

    private PlayerShooting _playerShooting;
    private int _currentAmmoIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerShooting = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerShooting>();
        _playerShooting.OnWeaponFired += (shotsFired) =>
        {
            UpdateAmmoUI(shotsFired);
        };
        _playerShooting.OnReloaded += (ammoReloaded) =>
        {
            for (int i = 0, a = 0; i < AmmoIcons.Count && a < ammoReloaded; i++)
            {
                if (!AmmoIcons[i].enabled)
                {
                    AmmoIcons[i].enabled = true;
                    a++;
                }
            }
            _currentAmmoIndex = 0;
        };
    }

    private void UpdateAmmoUI(int shotsFired)
    {
        AmmoIcons[_currentAmmoIndex].enabled = false;
        _currentAmmoIndex = GetNextAmmo(shotsFired);
    }

    private int GetNextAmmo(int increment)
    {
        int nextIndex = _currentAmmoIndex + increment;
        return Mathf.Clamp(nextIndex, 0, AmmoIcons.Count - 1);
    }
}

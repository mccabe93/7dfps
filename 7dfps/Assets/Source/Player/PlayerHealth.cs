using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    public Action<float> OnHealthChanged;

    public float Health = 10;
    public float HealthMax = 10;
    public FlashEffectUI FlashEffect;

    private bool _isDead;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        OnHealthChanged?.Invoke(Health);
        if (Health <= 0 && !_isDead)
        {
            Die();
        }
        if (FlashEffect != null && !FlashEffect.IsPlaying)
        {
            if (FlashEffect.UIImage == null)
            {
                FlashEffect.UIImage = GameObject
                    .FindGameObjectWithTag("UI_Overlay")
                    .GetComponent<UnityEngine.UI.Image>();
            }
            FlashEffect.StartFX();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet_Enemy")
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            TakeDamage(bullet.Damage);
        }
    }

    public void Die()
    {
        _isDead = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        var playerMovement = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponentInChildren<PlayerMovement>();
        GameObject.Destroy(playerMovement);

        var playerShooting = GameObject
            .FindGameObjectWithTag("Player")
            .GetComponentInChildren<PlayerShooting>();

        GameObject.Destroy(playerShooting);

        GameObject
            .FindGameObjectWithTag("LevelController")
            .GetComponent<LevelController>()
            .ShowRestartMenu();
    }
}

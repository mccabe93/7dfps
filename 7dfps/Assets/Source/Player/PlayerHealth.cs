using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    public float Health = 10;
    public float HealthMax = 10;
    public FlashEffectUI FlashEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet_Enemy")
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            Health -= bullet.Damage;
            if (Health <= 0)
            {
                Debug.Log("Player is dead!");
            }
            if (FlashEffect != null && !FlashEffect.IsPlaying)
            {
                FlashEffect.StartFX();
            }
        }
    }
}

using System;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyActor : MonoBehaviour
{
    public Action<Collision> OnCollision;
    public Action OnDeath;

    public Animator Animator;
    public float Health = 10f;

    public Texture2D[] BloodTextures;
    public FlashEffect FlashVfx;
    public FadeEffect FadeVfx;

    public bool IsAlive = true;

    void Awake()
    {
        if (FadeVfx != null)
        {
            FadeVfx.OnEffectComplete = () => Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsAlive && collision.gameObject.tag == "Bullet_Player")
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            Health -= bullet.Damage;
            if (Health <= 0f)
            {
                if (FlashVfx != null)
                {
                    FlashVfx.IsPlaying = false;
                }
                if (FadeVfx != null)
                {
                    FadeVfx.StartFX();
                }
                IsAlive = false;
                OnDeath?.Invoke();
            }
            else if (FlashVfx != null && !FlashVfx.IsPlaying)
            {
                FlashVfx.StartFX();
            }
        }
        OnCollision?.Invoke(collision);
    }
}

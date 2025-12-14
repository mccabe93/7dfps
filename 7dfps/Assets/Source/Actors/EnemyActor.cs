using System;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;

public class EnemyActor : MonoBehaviour
{
    public Action<Collision> OnHit;
    public Action<Collision> OnCollision;
    public Action OnDeath;

    public float Health = 10f;
    public FlashEffect FlashVfx;
    public FadeEffect FadeVfx;

    public bool IsAlive = true;

    void Awake()
    {
        var enemyListUI = GameObject
            .FindGameObjectWithTag("UI_EnemyList")
            .GetComponent<EnemyListUI>();
        enemyListUI.AddEnemy(gameObject);
        if (FadeVfx != null)
        {
            FadeVfx.OnEffectComplete = () => Destroy(gameObject);
        }
    }

    private void Reset()
    {
        FlashVfx = GetComponent<FlashEffect>();
        FadeVfx = GetComponent<FadeEffect>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision);
        if (IsAlive && collision.gameObject.tag == "Bullet_Player")
        {
            OnHit?.Invoke(collision);
            var bullet = collision.gameObject.GetComponent<Bullet>();
            Health -= bullet.Damage;
            if (Health <= 0f)
            {
                Die();
            }
            else if (FlashVfx != null && !FlashVfx.IsPlaying)
            {
                FlashVfx.StartFX();
            }
        }
    }

    public void Die()
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
}

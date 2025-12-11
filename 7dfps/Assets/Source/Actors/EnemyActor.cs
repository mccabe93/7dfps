using System;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public Action<Collision> OnCollision;

    public Animator Animator;
    public int Health = 10;

    private FlashEffect _flashVfx;
    private FadeEffect _fadeVfx;

    void Awake()
    {
        _fadeVfx = this.AddComponent<FadeEffect>();
        _fadeVfx.OnEffectComplete = () => Destroy(gameObject);
        _fadeVfx.Duration = 3.0f;
        _fadeVfx.Parent = gameObject;
        _flashVfx = this.AddComponent<FlashEffect>();
        _flashVfx.Color = Color.red;
        _flashVfx.Duration = 1.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet_Player")
        {
            Health -= 2;
            if (Health <= 0 && !_fadeVfx.IsPlaying)
            {
                _fadeVfx.StartFX();
            }
            else if (!_fadeVfx.IsPlaying && !_flashVfx.IsPlaying)
            {
                _flashVfx.StartFX();
            }
        }
        OnCollision?.Invoke(collision);
    }
}

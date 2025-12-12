using System;
using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    public Renderer Renderer;
    public Action OnEffectComplete { get; set; }
    public float Duration = 0.3f;
    public bool IsPlaying = false;

    public Color Color = Color.red;

    public bool StartFX()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
            StartCoroutine(FlashFX());
            return true;
        }
        return false;
    }

    private IEnumerator FlashFX()
    {
        float elapsedTime = 0.0f;
        Color originalColor = Renderer.material.color;
        float rDiff = originalColor.r - Color.r;
        float gDiff = originalColor.g - Color.g;
        float bDiff = originalColor.b - Color.b;
        while (elapsedTime < Duration)
        {
            if (!IsPlaying)
            {
                break;
            }
            elapsedTime += 0.1f;
            float delta = Mathf.Lerp(1.0f, 0.0f, elapsedTime / Duration);
            Renderer.material.color = new Color(
                originalColor.r - (rDiff * delta),
                originalColor.g - (gDiff * delta),
                originalColor.b - (bDiff * delta)
            );
            yield return new WaitForSeconds(0.1f);
        }
        IsPlaying = false;
        OnEffectComplete?.Invoke();
    }
}

using System;
using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour, IEffect
{
    public Action OnEffectComplete { get; set; }

    public GameObject Parent { get; set; }
    public float Duration { get; set; } = 3.0f;
    public bool IsPlaying { get; set; } = false;

    public bool StartFX()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
            StartCoroutine(FadeFX());
            return true;
        }
        return false;
    }

    private IEnumerator FadeFX()
    {
        float elapsedTime = 0.0f;
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        while (elapsedTime < Duration)
        {
            elapsedTime += 0.1f;
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / Duration);
            renderer.material.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );
            yield return new WaitForSeconds(0.1f);
        }
        OnEffectComplete?.Invoke();
    }
}

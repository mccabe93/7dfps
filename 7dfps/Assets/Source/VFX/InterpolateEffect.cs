using System;
using System.Collections;
using UnityEngine;

public class InterpolateEffect : MonoBehaviour
{
    public Transform Start;
    public Transform End;
    public GameObject Object;
    public float Duration = 1.0f;

    public Action OnEffectComplete { get; set; }
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
        while (elapsedTime < Duration)
        {
            if (!IsPlaying)
            {
                break;
            }
            elapsedTime += 0.1f;
            Object.transform.position = Vector3.Lerp(
                Start.position,
                End.position,
                elapsedTime / Duration
            );
            yield return new WaitForSeconds(0.1f);
        }
        IsPlaying = false;
        OnEffectComplete?.Invoke();
    }
}

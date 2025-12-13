using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class InterpolateEffect : MonoBehaviour
{
    public Transform StartPosition;
    public Transform EndPosition;
    public GameObject Object;
    public float Duration = 1.0f;
    public float Interval = 0.01f;

    public Action OnEffectComplete { get; set; }
    public bool IsPlaying { get; set; } = false;

    private WaitForSeconds _waiter;

    void Start()
    {
        _waiter = new WaitForSeconds(Interval);
    }

    public bool StartFX()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
            StartCoroutine(InterpolateFX());
            return true;
        }
        return false;
    }

    private IEnumerator InterpolateFX()
    {
        Vector3 originalPosition = Object.transform.localPosition;
        Quaternion originalRotation = Object.transform.localRotation;
        Object.transform.localPosition = StartPosition.localPosition;
        Object.transform.localRotation = StartPosition.localRotation;
        float elapsedTime = 0.0f;
        float totalElapsedTime = 0f;
        bool startedReverseInterpolation = false;
        float halfLife = Duration / 2f;
        while (totalElapsedTime < Duration)
        {
            if (!IsPlaying)
            {
                break;
            }
            elapsedTime += Interval;
            totalElapsedTime += Interval;
            if (totalElapsedTime >= halfLife)
            {
                if (!startedReverseInterpolation)
                {
                    elapsedTime = Interval;
                    startedReverseInterpolation = true;
                }
                Object.transform.localPosition = Vector3.Lerp(
                    EndPosition.localPosition,
                    StartPosition.localPosition,
                    elapsedTime / halfLife
                );
                Object.transform.localRotation = Quaternion.Lerp(
                    EndPosition.localRotation,
                    StartPosition.localRotation,
                    elapsedTime / halfLife
                );
                yield return _waiter;
            }
            else
            {
                Object.transform.localPosition = Vector3.Lerp(
                    StartPosition.localPosition,
                    EndPosition.localPosition,
                    elapsedTime / halfLife
                );
                Object.transform.localRotation = Quaternion.Lerp(
                    StartPosition.localRotation,
                    EndPosition.localRotation,
                    elapsedTime / halfLife
                );
                yield return _waiter;
            }
        }
        Object.transform.localPosition = originalPosition;
        Object.transform.localRotation = originalRotation;
        IsPlaying = false;
        OnEffectComplete?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    public float TransitionTime = 3.0f;
    public List<TextMeshProUGUI> Messages;
    private System.Action<TextMeshProUGUI> OnColorTransitioned;
    private readonly List<TextMeshProUGUI> _awaitingNewTransition = new List<TextMeshProUGUI>();
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(0.1f);

    public void Initialize()
    {
        foreach (var message in Messages)
        {
            OnColorTransitioned += (text) =>
            {
                if (_awaitingNewTransition.Contains(text))
                    return;
                _awaitingNewTransition.Add(message);
            };
            StartCoroutine(RandomColorizer(message));
        }
    }

    private void Update()
    {
        if (_awaitingNewTransition.Count > 0)
        {
            for (int i = _awaitingNewTransition.Count - 1; i >= 0; i--)
            {
                var text = _awaitingNewTransition[i];
                _awaitingNewTransition.RemoveAt(i);
                StartCoroutine(RandomColorizer(text));
            }
        }
    }

    private IEnumerator RandomColorizer(TextMeshProUGUI text)
    {
        Color c = new Color(Random.value, Random.value, Random.value);

        float elapsedTime = 0.0f;
        Color originalColor = text.color;
        float rDiff = originalColor.r - c.r;
        float gDiff = originalColor.g - c.g;
        float bDiff = originalColor.b - c.b;
        while (elapsedTime < TransitionTime)
        {
            elapsedTime += 0.1f;
            float delta = Mathf.Lerp(1.0f, 0.0f, elapsedTime / TransitionTime);
            text.color = new Color(
                originalColor.r - (rDiff * delta),
                originalColor.g - (gDiff * delta),
                originalColor.b - (bDiff * delta)
            );
            yield return _waitForSeconds;
        }
        OnColorTransitioned?.Invoke(text);
    }
}

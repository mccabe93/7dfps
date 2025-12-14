using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    public List<HealthIcon> HealthIcons;

    public float BaseAnimationInterval = 2f;
    public float MinimumAnimationInterval = 0.1f;

    private int _currentHealthIconIndex;
    private PlayerHealth _playerHealth;

    private bool _isAnimating = false;
    private float _animationInterval;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animationInterval = BaseAnimationInterval;
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        _playerHealth.OnHealthChanged += (currentHealth) =>
        {
            HealthIcon activeIcon = HealthIcons.FirstOrDefault(t =>
                currentHealth < t.MaximumHealth && currentHealth >= t.MinimumHealth
            );

            _animationInterval = Math.Max(
                BaseAnimationInterval * (currentHealth / _playerHealth.HealthMax),
                MinimumAnimationInterval
            );
            if (!activeIcon.IsActive)
            {
                activeIcon.Icon.enabled = true;
                activeIcon.IsActive = true;
                _currentHealthIconIndex = HealthIcons.IndexOf(activeIcon);
            }

            for (int i = 0; i < HealthIcons.Count; i++)
            {
                if (i != _currentHealthIconIndex)
                {
                    HealthIcons[i].IsActive = false;
                }
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isAnimating && _currentHealthIconIndex < HealthIcons.Count - 1)
        {
            _isAnimating = true;
            StartCoroutine("AnimationHealthIcon");
        }
    }

    private IEnumerator AnimationHealthIcon()
    {
        yield return new WaitForSeconds(_animationInterval);
        if (this.isActiveAndEnabled)
        {
            if (
                HealthIcons[_currentHealthIconIndex].Icon.enabled
                && _currentHealthIconIndex + 1 < HealthIcons.Count
            )
            {
                HealthIcons[_currentHealthIconIndex].Icon.enabled = false;
                HealthIcons[_currentHealthIconIndex + 1].Icon.enabled = true;
            }
            else
            {
                HealthIcons[_currentHealthIconIndex].Icon.enabled = true;
                HealthIcons[_currentHealthIconIndex + 1].Icon.enabled = false;
            }
        }
        _isAnimating = false;
    }
}

[Serializable]
public class HealthIcon
{
    public Image Icon;
    public float MinimumHealth;
    public float MaximumHealth;
    public bool IsActive;
}

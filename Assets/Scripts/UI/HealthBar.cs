using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    private TextMeshProUGUI healthText;

    private void Awake()
    {
        healthText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        UpdateHealthText(health);
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        UpdateHealthText(health);
    }

    private void UpdateHealthText(int health)
    {
        if (healthText != null)
        {
            healthText.text = health.ToString();
        }
    }
}

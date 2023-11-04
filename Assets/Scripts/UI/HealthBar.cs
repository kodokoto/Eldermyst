using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public TextMeshProUGUI healthText;


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
            Debug.Log("Update text");
            if (health == 0)
            {
                healthText.text = null;
            }
            else
            {
                healthText.text = health.ToString();
            }
        }
    }
}

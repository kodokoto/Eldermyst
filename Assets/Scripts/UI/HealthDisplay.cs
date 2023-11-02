using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    public void UpdateHealthText(int health)
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health.ToString();
        }
    }
}
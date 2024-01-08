using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxHealth(int health)
    {
        Debug.Log("Setting max health to " + health);
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        Debug.Log("Setting health to " + health);
        slider.value = health;
    }
}

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
        // increase gameobject length based on how health is set
        gameObject.transform.localScale = new Vector3(health / 100f, 1f, 1f);
    }

    public void SetHealth(int health)
    {
        Debug.Log("Setting health to " + health);
        slider.value = health;
    }
}

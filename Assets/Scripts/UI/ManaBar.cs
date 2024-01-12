using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        // increase gameobject length based on how health is set
        gameObject.transform.localScale = new Vector3(mana / 100f, 1f, 1f);
    }

    public void SetMana(int mana)
    {
        slider.value = mana;
    }
}

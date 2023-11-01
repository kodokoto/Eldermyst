using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxXP(int xp)
    {
        slider.maxValue = xp;
    }

    public void SetXP(int xp)
    {
        slider.value = xp;
    }
}

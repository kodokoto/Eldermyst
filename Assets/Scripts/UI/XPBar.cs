using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;
    // TMP Text
    private TMP_Text text;

    public void Awake()
    {
        text = gameObject.GetComponentInChildren<TMP_Text>();
    }

    public void SetMaxXP(int xp)
    {
        Debug.Log("Setting max XP to " + xp);
        slider.maxValue = xp;
    }

    public void SetXP(int xp)
    {
        Debug.Log("Setting XP to " + xp);
        slider.value = xp;
    }

    public void SetLevel(int level)
    {
        text.text = level + 1 + "";
    }
}
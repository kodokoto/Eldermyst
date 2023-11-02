using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    
    [Header("Stats")]
    public int maxHealth;
    public int maxMana;
    public int[] xpLevels = new int[] { 10, 20, 30, 40, 50 };

    public int healthRegen;
    public float healthRegenRate;
    public int manaRegen;
    public float manaRegenRate;

    // Ingame stats
    [HideInInspector] public int health;
    [HideInInspector] public int mana;
    [HideInInspector] public bool isShielded;
    [HideInInspector] public int xp;
    [HideInInspector] public int currentXPLevel;

    void OnValidate() 
    {
        health = maxHealth;
        mana = maxMana;
        xp = 0;
        currentXPLevel = 0;
    }

    public void Reset()
    {
        health = maxHealth;
        mana = maxMana;
        xp = 0;
        currentXPLevel = 0;
    }
}

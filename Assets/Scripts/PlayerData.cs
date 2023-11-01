using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    
    [Header("Stats")]
    public int maxHealth;
    public int maxMana;
    public int maxXP;

    public int healthRegen;
    public float healthRegenRate;
    public int manaRegen;
    public float manaRegenRate;

    // Ingame stats
    [HideInInspector] public int health;
    [HideInInspector] public int mana;
    [HideInInspector] public bool isShielded;
    [HideInInspector] public int xp;

    void OnValidate() 
    {
        health = maxHealth;
        mana = maxMana;
        xp = 0;
    }

    public void Reset()
    {
        health = maxHealth;
        mana = maxMana;
        xp = 0;
    }
}

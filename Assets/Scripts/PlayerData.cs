using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    
    [Header("Stats")]
    public int maxHealth;
    public int maxMana;
    public int healthRegen;
    public float healthRegenRate;
    public int manaRegen;
    public float manaRegenRate;

    // Ingame stats
    [HideInInspector] public int health;
    [HideInInspector] public int mana;
    [HideInInspector] public bool isShielded;
    
    void OnValidate() 
    {
        health = maxHealth;
        mana = maxMana;
    }

    public void Reset()
    {
        health = maxHealth;
        mana = maxMana;
    }
}

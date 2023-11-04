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
    public int[] xpLevels = new int[] { 10, 20, 30, 40, 50 };
    public int levelUpHealthRate = 10;
    public int levelUpManaRate = 10;

    // Ingame stats
    [HideInInspector] public int health;
    [HideInInspector] public int mana;
    [HideInInspector] public bool isShielded;
    [HideInInspector] public int currentXp;
    [HideInInspector] public int currentXpLevel;
    
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

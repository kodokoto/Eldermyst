using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PlayerData")]

public class PlayerData : SerializableScriptableObject
{
    
    public string playerName = "Player";

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
    [HideInInspector] public int excessHealth;

    public void SetPlayerData(PlayerData newData)
    {
        playerName = newData.playerName;
        maxHealth = newData.maxHealth;
        maxMana = newData.maxMana;
        healthRegen = newData.healthRegen;
        healthRegenRate = newData.healthRegenRate;
        manaRegen = newData.manaRegen;
        manaRegenRate = newData.manaRegenRate;
        xpLevels = newData.xpLevels;
        levelUpHealthRate = newData.levelUpHealthRate;
        levelUpManaRate = newData.levelUpManaRate;

        health = newData.health;
        mana = newData.mana;
        isShielded = newData.isShielded;
        currentXp = newData.currentXp;
        currentXpLevel = newData.currentXpLevel;
        excessHealth = newData.excessHealth;
    }

    // void OnValidate() 
    // {
    //     health = maxHealth;
    //     mana = maxMana;
    // }

    public void HardReset()
    {
        currentXpLevel = 0;
        SoftReset();
    }

    public void SoftReset()
    {
        health = maxHealth;
        mana = maxMana;
        excessHealth = 0;
        currentXp = 0;
    }
}

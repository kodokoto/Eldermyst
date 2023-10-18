using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    
    [Header("Stats")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float maxMana;

    [SerializeField] public int healthRegen;
    [SerializeField] public float healthRegenRate;
    [SerializeField] public int manaRegen;
    [SerializeField] public float manaRegenRate;

    // Ingame stats
    [HideInInspector] public float health;
    [HideInInspector] public float mana;
    [HideInInspector] public bool isShielded;
    
    void OnValidate() 
    {
        health = maxHealth;
        mana = maxMana;
    }
}

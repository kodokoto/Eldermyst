using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    
    [Header("Stats")]
    [SerializeField] public float maxHealth;
    [SerializeField] public float maxMana;

    [SerializeField] public float healthRegen;

    [SerializeField] public float manaRegen;




    private float health;
    private float mana;

    [HideInInspector] public bool isShielded;
    
    void OnValidate() 
    {
        health = maxHealth;
        mana = maxMana;
    }
}

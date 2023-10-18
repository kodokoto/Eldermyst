using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData data;
    [SerializeField] private Transform projectileSpawnPoint;
    private float healthRegenTimer;
    private float manaRegenTimer;

    public int getMana()
    {
        return data.mana;
    }

    public int getHealth()
    {
        return data.health;
    }

    public bool isShielded()
    {
        return data.isShielded;
    }

    public void setIsShielded(bool isShielded)
    {
        data.isShielded = isShielded;
    }

    public void takeDamage(int damage)
    {
        if (!data.isShielded)
        {
            data.health -= damage;
        }
    }

    public void consumeMana(int mana)
    {
        data.mana -= mana;
    }

    public void restoreHealth(int healAmount)
    {
        data.health += healAmount;
    }

    public void restoreMana(int mana)
    {
        data.mana += mana;
    }

    public void healthRegen()
    {
        if (data.health < data.maxHealth)
        {
            if (healthRegenTimer > data.healthRegenRate)
            {
                restoreHealth(data.healthRegen);
                healthRegenTimer = 0;
            }
            else
            {
                healthRegenTimer += Time.deltaTime;
            }
        }
    }

    public void manaRegen()
    {
        if (data.mana < data.maxMana)
        {
            if (manaRegenTimer > data.manaRegenRate)
            {
                restoreMana(data.manaRegen);
                manaRegenTimer = 0;
            }
            else
            {
                manaRegenTimer += Time.deltaTime;
            }
        }
    }

    public Transform getProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }

    void Update()
    {
        healthRegen();
        manaRegen();
    }
}

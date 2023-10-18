using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerData data;
    [SerializeField] private Transform projectileSpawnPoint;
    private float healthRegenTimer;
    private float manaRegenTimer;

    void Update()
    {
        healthRegen();
        manaRegen();
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


    // Helpers

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
             removeHealth(damage);
        }
    }

    public void consumeMana(int mana)
    {
        removeMana(mana);
    }

    public void restoreHealth(int healAmount)
    {
        addHealth(healAmount);
    }

    public void restoreMana(int mana)
    {
        addMana(mana);
    }
    public Transform getProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }

    // private data modifiers

    private void addHealth(int amount)
    {
        data.health = Mathf.Min(data.health + amount, data.maxHealth);
    }

    private void removeHealth(int amount)
    {
        data.health = Mathf.Max(data.health - amount, 0);
    }

    private void addMana(int amount)
    {
        data.mana = Mathf.Min(data.mana + amount, data.maxMana);
    }

    private void removeMana(int amount)
    {
        data.mana = Mathf.Max(data.mana - amount, 0);
    }

}

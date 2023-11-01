using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Alive,
    Dead
}

public class Player : MonoBehaviour , ITakeDamage
{
    public PlayerData data;
    public PlayerState state;
    [SerializeField] private Transform projectileSpawnPoint;
    private float healthRegenTimer;
    private float manaRegenTimer;

    // UI
    public HealthBar healthBar;
    public ManaBar manaBar;
    public XPBar xpBar;

    void Start()
    {
        data.Reset();
        healthBar.SetMaxHealth(data.maxHealth);
        manaBar.SetMaxMana(data.maxMana);
        xpBar.SetMaxXP(data.maxXP);
    }

    void Update()
    {
        HealthRegen();
        ManaRegen();
        CheckIfOutOfBounds();
    }

    public void CheckIfOutOfBounds()
    {
        if (transform.position.y < -30)
        {
            GameManager.instance.SetGameState(GameState.Lost);
            state = PlayerState.Dead;
        }
    }

    public void HealthRegen()
    {
        if (data.health < data.maxHealth)
        {
            if (healthRegenTimer > data.healthRegenRate)
            {
                RestoreHealth(data.healthRegen);
                healthRegenTimer = 0;
            }
            else
            {
                healthRegenTimer += Time.deltaTime;
            }
        }
    }

    public void ManaRegen()
    {
        if (data.mana < data.maxMana)
        {
            if (manaRegenTimer > data.manaRegenRate)
            {
                RestoreMana(data.manaRegen);
                manaRegenTimer = 0;
            }
            else
            {
                manaRegenTimer += Time.deltaTime;
            }
        }
    }


    // Helpers

    public int GetMana()
    {
        return data.mana;
    }

    public int GetHealth()
    {
        return data.health;
    }

    public int GetXP()
    {
        return data.xp;
    }

    public bool IsShielded()
    {
        return data.isShielded;
    }

    public void SetIsShielded(bool isShielded)
    {
        data.isShielded = isShielded;
    }

    public void TakeDamage(int damage)
    {
        if (!data.isShielded)
        {
             RemoveHealth(damage);
        }
    }

    public void ConsumeMana(int mana)
    {
        RemoveMana(mana);
    }

    public void RestoreHealth(int healAmount)
    {
        AddHealth(healAmount);
    }

    public void RestoreMana(int mana)
    {
        AddMana(mana);
    }

    public void GainXP(int amount)
    {
        AddXP(amount);
    }
    public Transform GetProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }

    // private data modifiers

    private void SetMaxHealth(int amount)
    {
        data.maxHealth = amount;
        data.health = Mathf.Min(data.health, data.maxHealth);
        healthBar.SetMaxHealth(data.maxHealth);
    }

    private void SetMaxMana(int amount)
    {
        data.maxMana = amount;
        data.mana = Mathf.Min(data.mana, data.maxMana);
        manaBar.SetMaxMana(data.maxMana);
    }

    private void SetMaxXP(int amount)
    {
        data.maxXP = amount;
        data.xp = Mathf.Min(data.xp, data.maxXP);
        xpBar.SetMaxXP(data.maxXP);
    }

    private void AddHealth(int amount)
    {
        if (state.Equals(PlayerState.Dead))
        {
            return;
        }
        data.health = Mathf.Min(data.health + amount, data.maxHealth);
        healthBar.SetHealth(data.health);
    }

    private void RemoveHealth(int amount)
    {
        if (state.Equals(PlayerState.Dead))
        {
            return;
        }
        data.health = Mathf.Max(data.health - amount, 0);
        healthBar.SetHealth(data.health);
        if (data.health <= 0)
        {
            Debug.Log("Health is 0");
            GameManager.instance.SetGameState(GameState.Lost);
            state = PlayerState.Dead;
        }
    }

    private void AddMana(int amount)
    {
        if (state.Equals(PlayerState.Dead))
        {
            return;
        }
        data.mana = Mathf.Min(data.mana + amount, data.maxMana);
        manaBar.SetMana(data.mana);
    }

    private void RemoveMana(int amount)
    {
        if (state.Equals(PlayerState.Dead))
        {
            return;
        }
        data.mana = Mathf.Max(data.mana - amount, 0);
        manaBar.SetMana(data.mana);
    }

    private void AddXP(int amount)
    {
        if(state.Equals(PlayerState.Dead))
        {
            return;
        }
        data.xp = Mathf.Min(data.xp + amount, data.maxXP);
        xpBar.SetXP(data.xp);
    }

}

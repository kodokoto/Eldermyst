using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    public SpellHandler shield;
    public SpellHandler projectile;
    public SpellHandler teleport;
    public GameObject SpellObj;

    public SpellHandler[] spells;



    void Start()
    {
        data.Reset();
        healthBar.SetMaxHealth(data.maxHealth);
        manaBar.SetMaxMana(data.maxMana);
        xpBar.SetMaxXP(data.xpLevels[data.currentXPLevel]);

        // set up spells
        spells = SpellObj.GetComponents<SpellHandler>();
        for (int i=0;i<3;i++) {
            Spell spell = spells[i].GetSpell();
            if (spell.GetName() == "Shield")
            {
                shield = spells[i];
            }
            else if (spell.GetName() == "Teleport")
            {
                teleport = spells[i];
            }
            else
            {
                projectile = spells[i];
            }
        }

        DisableSpell(teleport);
        DisableSpell(shield);
        EnableSpell(projectile);
     
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

    public int GetCurrentXPLevel()
    {
        return data.currentXPLevel;
    }

    public int GetXPLevel()
    {
        return data.xpLevels[GetCurrentXPLevel()];
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
        
        if ((amount + data.xp >= GetXPLevel())&& (GetCurrentXPLevel()<4))
        {
            int leftover = amount + data.xp - GetXPLevel();
            LevelUp(leftover);            
        }
        else
        {
            data.xp +=amount;
            xpBar.SetXP(data.xp);
        }
    }
    private void LevelUp(int leftover) {
        data.xp = leftover;
        data.currentXPLevel = GetCurrentXPLevel() + 1;
        xpBar.SetXP(data.xp);
        xpBar.SetMaxXP(GetXPLevel());

        switch (GetCurrentXPLevel())
        {
            case 1: EnableSpell(teleport);
            break;

            case 2: EnableSpell(shield);
               break;
        }
    }

    private void EnableSpell(SpellHandler spell)
    {
        spell.enabled = true;
        // UI 
    }

    private void DisableSpell(SpellHandler spell)
    {
        spell.enabled = false;
    }
}

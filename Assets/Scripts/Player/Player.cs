using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour , ITakeDamage, IGhost
{

    [Header("Data")]
    [SerializeField] public PlayerSpawnPoint currentSpawnPoint;
    [SerializeField] public PlayerData data;
    [SerializeField] public PlayerInventory PlayerInventory;
    [SerializeField] private SpellSignalSO _spellAquiredSignal;
    [SerializeField] private Transform projectileSpawnPoint;
    private float healthRegenTimer;
    private float manaRegenTimer;
    
    [Header("UI")]
    public HealthBar healthBar;
    public ManaBar manaBar;
    public XPBar xpBar;

    public List<SpellHandler> SpellHandlers;

    // Broadcasts
    [SerializeField] private SignalSO _onPlayerDeath;

    // State
    public bool IsGhost{ get; set; } = false;


    void Start()
    {
        SetUpSpells();
        healthBar.SetMaxHealth(data.maxHealth);
        healthBar.SetHealth(data.health);
        manaBar.SetMaxMana(data.maxMana);
        manaBar.SetMana(data.mana);
        xpBar.SetMaxXP(data.xpLevels[data.currentLevel]);
        xpBar.SetXP(data.currentXp);
        xpBar.SetLevel(data.currentLevel);
        transform.position = currentSpawnPoint.GetSpawnPoint();
        SetHealth(data.health); // this is to make sure that the health is not greater than the max health
    }
   
    void Update()
    {
        HealthRegen();
        ManaRegen();
    }

    private void SetUpSpells()
    {
        Debug.Log("Setting up spells");
        SpellHandlers = new List<SpellHandler>();
        foreach (Spell spell in PlayerInventory.Spells)
        {
            SpellHandler spellHandler = gameObject.AddComponent<SpellHandler>();
            spellHandler.Spell = spell;
            SpellHandlers.Add(spellHandler);
            _spellAquiredSignal.Trigger(spellHandler);
        }
    }

    private void HealthRegen()
    {
        if (data.health < data.maxHealth)
        {
            if (healthRegenTimer > data.healthRegenRate)
            {
                Heal(data.healthRegen);
                healthRegenTimer = 0;
            }
            else
            {
                healthRegenTimer += Time.deltaTime;
            }
        }
    }

    private void ManaRegen()
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

    public bool IsShielded()
    {
        return data.isShielded;
    }

    public void SetIsShielded(bool isShielded)
    {
        data.isShielded = isShielded;
    }

    public void TakeDamage(int damage, bool showEffect = true)
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

    public void Heal(int healAmount)
    {
        AddHealth(healAmount);
    }

    // use to reduce health for logic purposes, not for damage
    public void ReduceHealth(int amount)
    {
        RemoveHealth(amount);
    }

    public void RestoreMana(int mana)
    {
        AddMana(mana);
    }

    public void AddXP(int xp)
    {
        Debug.Log("Xp before: " + data.currentXp);
        Debug.Log("Adding " + xp + " xp");
        data.currentXp += xp;
        Debug.Log("Xp after: " + data.currentXp);
        if (data.currentXp >= data.xpLevels[data.currentLevel])
        {
            LevelUp();
        }
        xpBar.SetXP(data.currentXp);
    }

    public Transform GetProjectileSpawnPoint()
    {
        return projectileSpawnPoint;
    }

    public int GetMaxHealth()
    {
        return data.maxHealth;
    }

    // private data modifiers
    private void LevelUp()
    {
        // if current level is max level, do nothing
        if (data.currentLevel >= data.xpLevels.Length)
        {
            return;
        }

        data.currentXp -= data.xpLevels[data.currentLevel];
        data.currentLevel++;
        SetMaxHealth(data.maxHealth + data.levelUpHealthRate);
        AddHealth(data.levelUpHealthRate);
        SetMaxMana(data.maxMana + data.levelUpManaRate);
        AddMana(data.levelUpManaRate);
        xpBar.SetMaxXP(data.xpLevels[data.currentLevel]);
        xpBar.SetLevel(data.currentLevel);
    }

    private void SetHealth(int health)
    {
        data.health = Mathf.Min(health, data.maxHealth);
        healthBar.SetHealth(data.health);
    }

    public void SetMaxHealth(int amount)
    {
        data.maxHealth = amount;
        healthBar.SetMaxHealth(data.maxHealth);
    }

    private void AddHealth(int amount)
    {
        SetHealth(data.health + amount);
    }

    private void RemoveHealth(int amount)
    {
        SetHealth(Mathf.Max(data.health - amount, 0));
        if (data.health <= 0)
        {
            Debug.Log("Health is 0");
            HandleDeath();
        }
    }

    private void SetMana(int mana)
    {
        data.mana = Mathf.Min(mana, data.maxMana);
        manaBar.SetMana(data.mana);
    }

    private void SetMaxMana(int amount)
    {
        data.maxMana = amount;
        manaBar.SetMaxMana(data.maxMana);
    }

    private void AddMana(int amount)
    {
        SetMana(data.mana + amount);
    }

    private void RemoveMana(int amount)
    {
        SetMana(Mathf.Max(data.mana - amount, 0));
    }

    private void HandleDeath()
    {
        _onPlayerDeath.Trigger();
    }

    internal void AddSpell(Spell spell)
    {
        PlayerInventory.AddSpell(spell);
        SpellHandler spellHandler = gameObject.AddComponent<SpellHandler>();
        spellHandler.Spell = spell;
        SpellHandlers.Add(spellHandler);
        _spellAquiredSignal.Trigger(spellHandler);
    }
}

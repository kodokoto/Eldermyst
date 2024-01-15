using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour , ITakeDamage, IGhost, IFreezable, IFiresProjectiles
{

    [Header("Data")]
    [SerializeField] public PlayerSpawnPoint currentSpawnPoint;
    [SerializeField] public PlayerData data;
    [SerializeField] public PlayerInventory PlayerInventory;
    [SerializeField] private SpellSignalSO _spellAquiredSignal;
    [field: SerializeField] public Transform ProjectileSpawnPoint { get; set; }

    [SerializeField] private AudioClip _damageSFX = default;

    private float healthRegenTimer;
    private float manaRegenTimer;
    
    [Header("UI")]
    public HealthBar healthBar;
    public ManaBar manaBar;
    public XPBar xpBar;

    public List<SpellHandler> SpellHandlers;

    [Header("Broadcasts")]
    // Broadcasts
    [SerializeField] private SignalSO _onPlayerDeath;
    [SerializeField] private AudioSignalSO _sfxAudioSignal;

    // State
    public bool IsGhost{ get; set; } = false;
    public bool IsFrozen { get; set; }

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
            spellHandler.audioSignalSO = _sfxAudioSignal;
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
             _sfxAudioSignal.Trigger(_damageSFX, transform.position, 20f);
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
            if (data.currentLevel >= data.xpLevels.Length -1)
            {
                
                return;
            }
            data.currentLevel++;
            data.currentXp -= data.xpLevels[data.currentLevel];
            LevelUp();
        }
        xpBar.SetXP(data.currentXp);
    }
    public int GetMaxHealth()
    {
        return data.maxHealth;
    }

    // private data modifiers
    private void LevelUp()
    {
        Debug.Log("Leveling up");

        // if current level is max level, do nothing

        SetMaxHealth(data.maxHealth + data.levelUpHealthRate);
        AddHealth(data.levelUpHealthRate);
        SetMaxMana(data.maxMana + data.levelUpManaRate);
        AddMana(data.levelUpManaRate);
        Debug.Log("Current level is now " + data.currentLevel);
        xpBar.SetMaxXP(data.xpLevels[data.currentLevel-1]);
        xpBar.SetLevel(data.currentLevel);

        Debug.Log("Max health is now " + data.maxHealth);
        Debug.Log("Max mana is now " + data.maxMana);
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
        spellHandler.audioSignalSO = _sfxAudioSignal;
        SpellHandlers.Add(spellHandler);
        _spellAquiredSignal.Trigger(spellHandler);
    }

    public void Freeze(int damage)
    {
        IsFrozen = true;
        TakeDamage(damage, false);
        StartCoroutine(FreezeEffect());
    }

    public void Unfreeze()
    {
        IsFrozen = false;
    }

    private IEnumerator FreezeEffect()
    {   
        Animator animator = GetComponent<Animator>();
        PlayerMovement movement = GetComponent<PlayerMovement>();
        MeshRenderer MeshRenderer = GetComponent<MeshRenderer>();

        animator.speed = 0;
        movement.Freeze();
        // save the original texture
        Texture c = MeshRenderer.material.GetTexture("_BaseMap");
        // delete the texture
        MeshRenderer.material.SetTexture("_BaseMap", null);

        // cache the colour
        Color c2 = MeshRenderer.material.GetColor("_BaseColor");
        // set the colour to blue
        MeshRenderer.material.SetColor("_BaseColor", new Color(93, 172, 177));
        // wait for freeze to be false
        while (IsFrozen)
        {
            yield return new WaitForSeconds(0.1f);
        }
        MeshRenderer.material.SetTexture("_BaseMap", c);
        MeshRenderer.material.SetColor("_BaseColor", c2);
        animator.speed = 1;
        movement.Unfreeze();
    }

}

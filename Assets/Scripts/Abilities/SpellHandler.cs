using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SpellState
{
    Ready,
    Active,
    Cooldown,
}

public class ComboManager
{
    private static List<KeyCode> ValidComboKeys = new List<KeyCode>(
        new KeyCode[] {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.E,
            KeyCode.Q,
        }
    );

    public List<KeyCode> Combo;

    private float ComboTimer = 0;
    private int CurrentComboStep = 0;
    private float MaxComboTime = 10f;

    public ComboManager(List<KeyCode> combo)
    {
        Combo = combo;
        ComboTimer = MaxComboTime;
    }
    // listen for key presses
    public void Listen()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            foreach (KeyCode key in ValidComboKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    Debug.Log("Combo key Pressed " + key);
                    AdvanceCombo(key);
                }
            }
        }
    }

    // advance combo
    public void AdvanceCombo(KeyCode key)
    {
        // if key is next in combo
        Debug.Log("Current Combo Step: " + CurrentComboStep);
        Debug.Log("Combo Step: " + Combo[CurrentComboStep]);
        Debug.Log("Key: " + key);
        if (key == Combo[CurrentComboStep])
        {
            Debug.Log("Key is Valid");
            CurrentComboStep++;
            Debug.Log("Current Combo Step: " + CurrentComboStep);
        }
        else
        {
            Debug.Log("Key is Invalid, Combo Reset");
            CurrentComboStep = 0;
        }
    }

    // called by spell handler in update
    public bool ComboCompleted()
    {
        Listen();
        if (ComboTimer > 0)
        {
            ComboTimer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Combo Timer Expired");
            CurrentComboStep = 0;
        }
        if (CurrentComboStep == Combo.Count)
        {
            Debug.Log("Combo Completed");
            CurrentComboStep = 0;
            ComboTimer = MaxComboTime;
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class SpellHandler : MonoBehaviour
{
    public Spell Spell;
    private SpellState state;
    private float CooldownTimer;
    private float ActiveTimer;
    ComboManager comboManager;

    private bool Casting = false;

    void Start()
    {
        state = SpellState.Ready;
        // comboManager = new ComboManager(Spell.combo);
    }

    void Update()
    {
        
        switch (state)
        {
            case SpellState.Ready:
                if (Casting && Spell.Validate(gameObject))
                {
                    // assert that the spell is not null
                    Debug.Assert(Spell != null, "Spell is null");

                    // assert that gameObject is not null
                    Debug.Assert(gameObject != null, "gameObject is null");
                    Spell.Activate(gameObject);
                    state = SpellState.Active;
                    ActiveTimer = Spell.activeTime;
                }
                break;
            case SpellState.Active:
                if (ActiveTimer > 0)
                {
                    ActiveTimer -= Time.deltaTime;
                }
                else
                {
                    Casting = false;
                    Spell.Deactivate(gameObject);
                    state = SpellState.Cooldown;
                    CooldownTimer = Spell.cooldownTime;
                }
                break;
            case SpellState.Cooldown:
                if (CooldownTimer > 0)
                {
                    CooldownTimer -= Time.deltaTime;
                }
                else
                {
                    state = SpellState.Ready;
                }
                break;
        }
    }

    public void SetSpell(Spell spell)
    {
        Spell = spell;
    }

    public void Cast()
    {
        if (state == SpellState.Ready)
        {
            Casting = true;
        }
    }
}
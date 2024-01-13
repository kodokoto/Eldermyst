using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SpellState
{
    Ready,
    Active,
    Cooldown,
}

public class SpellHandler : MonoBehaviour
{
    public Spell Spell;
    private SpellState state;
    public float CooldownTimer;
    private float ActiveTimer;
    private bool Casting = false;

    void Start()
    {
        state = SpellState.Ready;
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

    public SpellState GetState()
    {
        return getState();
    }

    private SpellState getState()
    {
        return state;
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    public Spell spell;
    float cooldownTime;
    float activeTime;
    public bool isAquired = false;
    public SpellCircle spellCircle;
    //public int levelRequirement;

    enum SpellState
    {
        Ready,
        Active,
        Cooldown,
    }

    private SpellState state;

    public KeyCode key;

    public void Start() {
        this.enabled = false;
    }

    public void Unlock() {
        this.enabled = true;
    }

    public void Lock()
    {
        this.enabled = false;
    }

    public string GetSpell()
    {
        return this.spell.name;
    }

    public void setKey(KeyCode Key)
    {
        key = Key;
    }

    public KeyCode getKey()
    {
        return key;
    }

    public bool GetIsAquired()
    {
        return isAquired;
    }

    public void setIsAquired()
    {
        isAquired = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spellCircle.isActive)
        {
            switch (state)
            {
                case SpellState.Ready:
                    if (Input.GetKeyDown(key) && spell.Validate(gameObject))
                    {
                        spell.Activate(gameObject);
                        state = SpellState.Active;
                        activeTime = spell.activeTime;
                    }
                    break;
                case SpellState.Active:
                    if (activeTime > 0)
                    {
                        activeTime -= Time.deltaTime;
                    }
                    else
                    {
                        spell.Deactivate(gameObject);
                        state = SpellState.Cooldown;
                        cooldownTime = spell.cooldownTime;
                    }
                    break;
                case SpellState.Cooldown:
                    if (cooldownTime > 0)
                    {
                        cooldownTime -= Time.deltaTime;
                    }
                    else
                    {
                        state = SpellState.Ready;
                    }
                    break;
            }
        }
    }
}

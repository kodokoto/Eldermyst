using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandler : MonoBehaviour
{
    public Spell spell;
    float cooldownTime;
    float activeTime;

    enum SpellState
    {
        Ready,
        Active,
        Cooldown,
    }

    private SpellState state;

    public KeyCode key;

    // Update is called once per frame
    void Update()
    {
        switch (state) {
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

    public Spell GetSpell(){
        return spell;
    }
}

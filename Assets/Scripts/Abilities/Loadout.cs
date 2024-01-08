using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Loadout : ScriptableObject
{
    // array of spells size 7
    [SerializeField] public List<Spell> Spells;
    [HideInInspector] public List<SpellHandler> SpellHandlers;

    // spell slot 1
    private SpellHandler slot1;
    private SpellHandler slot2;

    public void AddSpell(Spell spell)
    {
        Spells.Add(spell);
    }

    public void SetSpellSlot1(SpellHandler spell)
    {
        slot1 = spell;
    }

    public void SetSpellSlot2(SpellHandler spell)
    {
        slot2 = spell;
    }

    public void Reset()
    {
        Debug.Log("Resetting loadout");
        Spells.RemoveRange(1, Spells.Count -1);
        Debug.Log("Spells count " + Spells.Count);
        SpellHandlers = new List<SpellHandler>();
    }    
}
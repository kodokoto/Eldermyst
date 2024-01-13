using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellKey
{
    Left='\u2190',
    Right= '\u2192',
    Up= '\u2191',
    Down= '\u2193',
    Attack='E',
    Defend='Q'
}

[System.Serializable]
public struct SpellCombo
{
    public List<SpellKey> keys;
}


public class Spell : SerializableScriptableObject
{
    public string spellName;
    public Sprite icon;
    public float cooldownTime;
    public float activeTime;
    public int manaCost;
    public List<SpellCombo> combos;
    [SerializeField] public List<string> sentences;

    public virtual void Activate(GameObject parent){}
    public virtual void Deactivate(GameObject parent){}

    public virtual bool IsValid(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        if (player.IsGhost) {
            return false;
        }
        return true;
    }

    public bool Validate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        if (player.GetMana() >= manaCost && IsValid(parent))
        {
            player.ConsumeMana(manaCost);
            return true;
        }
        else
        {
            return false;
        }
    }  
}

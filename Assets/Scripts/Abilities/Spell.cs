using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellKey
{
    Left,
    Right,
    Up,
    Down,
    Attack,
    Defend
}

[System.Serializable]
public struct SpellCombo
{
    public List<SpellKey> keys;
}


public class Spell : ScriptableObject
{
    public string spellName;
    public Sprite icon;
    public float cooldownTime;
    public float activeTime;
    public int manaCost;
    public int levelRequired;
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

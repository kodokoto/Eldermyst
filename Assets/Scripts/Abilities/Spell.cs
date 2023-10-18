using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : ScriptableObject
{
    public new string name;
    public float cooldownTime;
    public float activeTime;
    public virtual int manaCost { get; set; }
    public virtual void Activate(GameObject parent){}
    public virtual void Deactivate(GameObject parent){}

    public virtual bool isValid(GameObject parent)
    {
        return true;
    }

    public bool Validate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        if (player.getMana() >= manaCost && isValid(parent))
        {
            player.consumeMana(manaCost);
            return true;
        }
        else
        {
            return false;
        }
    }
}

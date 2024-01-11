using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PlayerInventory")]
public class PlayerInventory : SerializableScriptableObject
{
    // array of spells size 7
    [SerializeField] public List<Spell> Spells;
    // spell slot 1
    public void AddSpell(Spell spell)
    {
        Spells.Add(spell);
    }

    public void Reset()
    {
        Debug.Log("Resetting PlayerInventory");
        // always keep the first spell (projectile)
        Spells.RemoveRange(1, Spells.Count -1);
        Debug.Log("Spells count " + Spells.Count);
    }    
}
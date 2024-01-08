using UnityEngine;

public class SpellScroll : Pickup
{
    public Spell spell;

    protected override void OnPickup(GameObject actor)
    {
        // Add spell to player's spell list
        actor.GetComponent<Player>().AddSpell(spell);
        Destroy(gameObject);
    }
}
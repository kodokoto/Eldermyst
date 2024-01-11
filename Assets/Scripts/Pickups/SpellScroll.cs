using UnityEngine;

public class SpellScroll : Pickup
{
    public Spell spell;
    [SerializeField] private DialogueSignalSO dialogueSignal;

    protected override void OnPickup(GameObject actor)
    {
        // Add spell to player's spell list
        actor.GetComponent<Player>().AddSpell(spell);
        dialogueSignal.Trigger(spell.sentences);
        Destroy(gameObject);
    }
}
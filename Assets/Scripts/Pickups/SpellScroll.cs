using UnityEngine;

public class SpellScroll : Pickup
{
    public Spell spell;
    [SerializeField] private DialogueDataChannelSO dialogueChannel;

    protected override void OnPickup(GameObject actor)
    {
        // Add spell to player's spell list
        actor.GetComponent<Player>().AddSpell(spell);
        dialogueChannel.RaiseEvent(spell.sentences);
        Destroy(gameObject);
    }
}
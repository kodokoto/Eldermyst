using UnityEngine;

public class SpellScroll : Pickup
{
    public Spell spell;
    [SerializeField] private DialogueSignalSO dialogueSignal;

    void Update()
    {
        gameObject.transform.Rotate(10.0f, 0.0f, 0.0f, Space.Self);
    }

    protected override void OnPickup(GameObject actor)
    {
        // Add spell to player's spell list
        actor.GetComponent<Player>().AddSpell(spell);
        dialogueSignal.Trigger(spell.sentences);
        Destroy(gameObject);
    }
}
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SpellScroll : Pickup
{
    public Spell spell;
    public Player player;
    public List<Spell> Spells;
    [SerializeField] private DialogueSignalSO dialogueSignal;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Spells = player.PlayerInventory.Spells;
        for (int i = 0; i < Spells.Count; i++)
        {
            if (Spells[i].spellName == spell.spellName)
            {
                Destroy(gameObject);
            }
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.up, 50 * Time.deltaTime);
    }

    protected override void OnPickup(GameObject actor)
    {
        // Add spell to player's spell list
        actor.GetComponent<Player>().AddSpell(spell);
        dialogueSignal.Trigger(spell.sentences);
        Destroy(gameObject);
    }
}
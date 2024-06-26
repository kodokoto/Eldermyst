using UnityEngine;

public class SpellScroll : Pickup
{
    public Spell spell;
    public PlayerInventory playerInventory;
    [SerializeField] private DialogueSignalSO dialogueSignal;
    private void Start()
    {
        // check if player already has spell
        if (playerInventory.HasSpell(spell))
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.up, 50 * Time.deltaTime);
        // check if player already has spell
        if (playerInventory.HasSpell(spell))
        {
            Destroy(gameObject);
        }
    }

    protected override void OnPickup(GameObject actor)
    {
        _pickupAudioSignal.Trigger(_pickupSFX, actor.transform.position, 50f);
        // Add spell to player's spell list
        actor.GetComponent<Player>().AddSpell(spell);
        dialogueSignal.Trigger(spell.sentences);
        Debug.Log("Playing SFX at " + transform.position + " with volume 50");
        Destroy(gameObject);
    }
}
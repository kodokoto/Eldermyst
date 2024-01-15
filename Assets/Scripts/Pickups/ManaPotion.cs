using UnityEngine;

public class ManaPotion : Pickup
{
    [SerializeField] private int ManaBoost;
    protected override void OnPickup(GameObject actor)
    {
        _pickupAudioSignal.Trigger(_pickupSFX, actor.transform.position, 50f);
        Player player = actor.GetComponent<Player>();
        player.RestoreMana(ManaBoost);
        Destroy(gameObject);
    }
}
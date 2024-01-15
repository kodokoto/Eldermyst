using UnityEngine;

public class HealthPotion : Pickup
{

    [SerializeField] private int HealthBoost;
    protected override void OnPickup(GameObject actor)
    { 
        _pickupAudioSignal.Trigger(_pickupSFX, actor.transform.position, 50f);
        Player player = actor.GetComponent<Player>();
        player.Heal(HealthBoost);
        Destroy(gameObject);
    }
}
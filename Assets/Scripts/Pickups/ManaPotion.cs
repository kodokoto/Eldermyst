using UnityEngine;

public class ManaPotion : Pickup
{
    [SerializeField] private int ManaBoost;
    protected override void OnPickup(GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        player.RestoreMana(ManaBoost);
        Destroy(gameObject);
    }
}
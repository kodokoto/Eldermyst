using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Pickup
{

    [SerializeField] private int HealthBoost;
    protected override void OnPickup(GameObject actor)
    {
        Player player = actor.GetComponent<Player>();
        player.RestoreHealth(HealthBoost);
        Destroy(gameObject);
    }
}
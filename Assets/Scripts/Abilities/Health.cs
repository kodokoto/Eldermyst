using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Health : Spell
{
    private int originalMaxHealth;

    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();

        if (player != null)
        {
            originalMaxHealth = player.GetMaxHealth();
            player.setMaxHealth(originalMaxHealth+originalMaxHealth);
            player.RestoreHealth(originalMaxHealth);
        }
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();

        if (player != null)
        {
            player.setMaxHealth(originalMaxHealth);
            player.TakeDamage(originalMaxHealth);
        }
    }
}
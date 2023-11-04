using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthBuff : Spell
{
    private int originalMaxHealth;

    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();

        if (player != null)
        {
            originalMaxHealth = player.GetMaxHealth();

            int increaseAmount = originalMaxHealth;
            player.IncreaseMaxHealth(increaseAmount);
            player.RestoreHealth(increaseAmount);
            if (player.GetHealth()==0)
            {
                player.DecreaseMaxHealth(originalMaxHealth);
            }
        }
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();

        if (player != null)
        {
            int decreaseAmount = originalMaxHealth;
            player.DecreaseMaxHealth(decreaseAmount);
            player.TakeDamage(decreaseAmount/2);
        }
    }
}

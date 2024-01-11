using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Health")]
public class Health : Spell
{
    private int OriginalHealth;
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        OriginalHealth = player.GetHealth();
        if ((player.GetHealth() + 20 > player.GetMaxHealth())){
            player.setExcess(player.GetMaxHealth() - (player.GetHealth() + 20));
        }
        player.RestoreHealth(50);
        
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.setHealth(OriginalHealth);
        
    }
}
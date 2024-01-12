using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Health")]
public class Health : Spell
{
    [SerializeField] private int _boostAmount = 50;
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.SetMaxHealth(player.GetMaxHealth() + _boostAmount);
        player.Heal(_boostAmount);
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.SetMaxHealth(player.GetMaxHealth() - _boostAmount);
    }
}
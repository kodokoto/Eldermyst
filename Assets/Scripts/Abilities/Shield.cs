using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Shield : Ability
{
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.data.isShielded = true;
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.data.isShielded = false;
    }
}

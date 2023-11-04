using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Shield : Spell
{

    public Color32 sheildTint = new(60, 240, 240, 240);

    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.SetIsShielded(true);
        // make the player capsule tinted
        player.GetComponent<Renderer>().material.color = sheildTint;
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.SetIsShielded(false);
        // set the player capsule to white
        player.GetComponent<Renderer>().material.color = Color.white;
    }
}

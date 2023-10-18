using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Shield : Spell
{

    public Color32 sheildTint = new Color32(60, 240, 240, 240);
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.data.isShielded = true;

        parent.GetComponent<SpriteRenderer>().color = sheildTint;
        Debug.Log(parent.GetComponent<SpriteRenderer>().color);
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.data.isShielded = false;

        parent.GetComponent<SpriteRenderer>().color = Color.white;
    }
}

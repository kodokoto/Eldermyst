using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Shield : Spell
{

    [SerializeField] public new int manaCost = 10;
    public Color32 sheildTint = new Color32(60, 240, 240, 240);
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.setIsShielded(true);
        parent.GetComponent<SpriteRenderer>().color = sheildTint;
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        player.setIsShielded(false);
        parent.GetComponent<SpriteRenderer>().color = Color.white;
    }
}

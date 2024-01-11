using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Spells/Ghost")]
public class Ghost : Spell
{
    public override void Activate(GameObject parent)
    {
        Debug.Log("Ghost spell activated");
        parent.GetComponent<IGhost>().Ghost();
        // make player transparent
        Renderer renderer = parent.GetComponent<Renderer>();
        renderer.material.color = new Color(
            renderer.material.color.r,
            renderer.material.color.g,
            renderer.material.color.b,
            0.2f
        );
        
    }

    public override void Deactivate(GameObject parent)
    {
        Debug.Log("Ghost spell deactivated");
        parent.GetComponent<IGhost>().UnGhost();
        Renderer renderer = parent.GetComponent<Renderer>();
        renderer.material.color = new Color(
            renderer.material.color.r,
            renderer.material.color.g,
            renderer.material.color.b,
            1f
        );
    }
}

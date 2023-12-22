using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Shield : Spell
{

    ParticleSystem particleSystem;
    
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        particleSystem = player.GetComponentInChildren<ParticleSystem>();
        player.SetIsShielded(true);
        // enable shield particle graphics
        var emission = particleSystem.emission;
        emission.enabled = true;
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        // disable shield particles
        var emission = particleSystem.emission;
        emission.enabled = false;
        player.SetIsShielded(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu]
public class Freeze : Spell
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask targetMask;
    
    private Collider[] collisionChecks;

    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        collisionChecks = Physics.OverlapSphere(player.transform.position, radius, targetMask);
        foreach (Collider c in collisionChecks)
        {
            if (c.TryGetComponent(out IFreezable enemy))
            {
                enemy.Freeze(damage);
            }
        }
    }

    public override void Deactivate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        collisionChecks = Physics.OverlapSphere(player.transform.position, radius, targetMask);
        foreach (Collider c in collisionChecks)
        {
            if (c.TryGetComponent(out IFreezable enemy))
            {
                enemy.Unfreeze();
            }
        }
    }

}
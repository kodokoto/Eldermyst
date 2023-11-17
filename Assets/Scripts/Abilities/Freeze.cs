using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[CreateAssetMenu]
public class Freeze : Spell
{

    private float radius = 10f;
    private Enemy enemy;
    public LayerMask targetMask;


    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        Collider[] collisionChecks = Physics.OverlapSphere(player.transform.position, radius, targetMask);
        if (collisionChecks.Length != 0)
        {
            enemy = collisionChecks[0].GetComponent<Enemy>();
            enemy.setFrozen(true);
            enemy.TakeDamage(2);
        }
    }

    public override void Deactivate(GameObject parent)
    {
        enemy.setFrozen(false);
    }

}
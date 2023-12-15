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
    private Collider[] collisionChecks;
    private IFreezable FreezeEnemy;


    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        collisionChecks = Physics.OverlapSphere(player.transform.position, radius, targetMask);
        if (collisionChecks.Length != 0)
        {
            for (int i=0;i<collisionChecks.Length; i++)
            {
                if (collisionChecks[i].GetComponent<Enemy>() == null)
                {
                    Debug.LogError($"{enemy} has the enemy tag but does not have the enemy component");
                }
                else
                {
                    enemy = collisionChecks[i].GetComponent<Enemy>();
                    FreezeEnemy = collisionChecks[i].GetComponent<Enemy>();
                    FreezeEnemy.Freeze();
                    enemy.TakeDamage(2);
                }
            }
        }
    }

    public override void Deactivate(GameObject parent)
    {
        for (int i = 0; i < collisionChecks.Length; i++)
        {
            FreezeEnemy = collisionChecks[i].GetComponent<Enemy>();
            FreezeEnemy.Unfreeze();
        }
    }

}
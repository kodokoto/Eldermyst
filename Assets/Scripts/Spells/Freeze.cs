using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Spells/Freeze")]
public class Freeze : Spell
{
    [SerializeField] private int damage = 20;
    [SerializeField] public float radius = 10f;
    [SerializeField] private LayerMask targetMask;
    public GameObject ObjectToSpawn;
    private GameObject explosion;

    private Collider[] collisionChecks;

    public override void Activate(GameObject parent)
    {
        Debug.Log("Freeze activated");
        // assert parent is not null
        Debug.Assert(parent != null, "Parent is null");

        Debug.Assert(ObjectToSpawn != null, "ObjectToSpawn is null");
        explosion = Instantiate(ObjectToSpawn, parent.transform.position, parent.transform.rotation);
        
        // assert explosion is not null
        Debug.Assert(explosion != null, "Explosion is null");
        collisionChecks = Physics.OverlapSphere(parent.transform.position, radius, targetMask);
        Debug.Log("Freeze collision checks " + collisionChecks.Length);
        foreach (Collider c in collisionChecks)
        {
            if (c.TryGetComponent(out IFreezable enemy))
            {
                Debug.Log("Freezing enemy");
                enemy.Freeze(damage);
            }
        }
    }

    public override void Deactivate(GameObject parent)
    {
        Destroy(explosion);
        foreach (Collider c in collisionChecks)
        {
            // incase the enemy is killed by the explosion
            if (c!= null)
            {
                if (c.TryGetComponent(out IFreezable enemy))
                {
                    enemy.Unfreeze();
                }
            }
        }
        // clear collision checks
        collisionChecks = null;
    }
}
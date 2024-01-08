using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.VFX;

[CreateAssetMenu]
public class Freeze : Spell
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float radius = 10f;
    [SerializeField] private LayerMask targetMask;
    public GameObject ObjectToSpawn;
    private GameObject explosion;
    public Material ice;
    public Material lava;


    private Collider[] collisionChecks;

    public override void Activate(GameObject parent)
    {
        // assert parent is not null
        Debug.Assert(parent != null, "Parent is null");

        Debug.Assert(ObjectToSpawn != null, "ObjectToSpawn is null");
        Player player = parent.GetComponent<Player>();
        explosion = Instantiate(ObjectToSpawn, player.transform.position, player.transform.rotation);

        // assert explosion is not null
        Debug.Assert(explosion != null, "Explosion is null");
        collisionChecks = Physics.OverlapSphere(player.transform.position, radius, targetMask);
        foreach (Collider c in collisionChecks)
        {
            if (c.TryGetComponent(out IFreezable enemy))
            {
                enemy.Freeze(damage);

                Renderer renderer = c.GetComponent<Renderer>();
                renderer.material = ice;
            }
        }
    }

    public override void Deactivate(GameObject parent)
    {
        Destroy(explosion);
        Player player = parent.GetComponent<Player>();
        collisionChecks = Physics.OverlapSphere(player.transform.position, radius, targetMask);
        foreach (Collider c in collisionChecks)
        {
            if (c.TryGetComponent(out IFreezable enemy))
            {
                enemy.Unfreeze();
                Renderer renderer = c.GetComponent<Renderer>();
                renderer.material = lava;
            }
        }
    }

}
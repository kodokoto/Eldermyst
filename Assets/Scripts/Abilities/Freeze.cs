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
        Player player = parent.GetComponent<Player>();
        Vector3 pos = new Vector3(player.transform.position.x, player.transform.position.y+2, player.transform.position.z);
        ObjectToSpawn.transform.position = pos;
        explosion = Instantiate(ObjectToSpawn);
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
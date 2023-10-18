using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProjectileSpell : Spell
{
    public GameObject projectilePrefab;
    public override void Activate(GameObject parent)
    {
        Transform spawnPoint = parent.transform.Find("ProjectileSpawnPoint");
        Instantiate(projectilePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public override void Deactivate(GameObject parent){}
}

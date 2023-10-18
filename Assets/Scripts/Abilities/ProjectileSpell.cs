using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ProjectileSpell : Spell
{
    public GameObject projectilePrefab;
    public override void Activate(GameObject parent)
    {
        Player player = parent.GetComponent<Player>();
        Transform projectileSpawnPoint = player.GetProjectileSpawnPoint();
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    public override void Deactivate(GameObject parent){}
}

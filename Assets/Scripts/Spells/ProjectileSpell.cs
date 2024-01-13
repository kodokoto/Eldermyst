using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/ProjectileSpell")]
public class ProjectileSpell : Spell
{
    public GameObject projectilePrefab;

    public override void Activate(GameObject parent)
    {
        IFiresProjectiles player = parent.GetComponent<IFiresProjectiles>();
        Transform projectileSpawnPoint = player.ProjectileSpawnPoint;
        Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    }

    public override void Deactivate(GameObject parent){}
}

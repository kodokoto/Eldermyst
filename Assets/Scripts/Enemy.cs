using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage
{



    public Transform projectileSpawnPoint;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public Projectile projectile;

    public int health = 20;
    public float fovRadius = 10f;
    public float fireRate = 0.2f;
    public int xpValue = 10;

    void Start()
    {
        obstructionMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("StickyWall");
        StartCoroutine(Routine());
    }

    public IEnumerator Routine()
    {
        // while the enem
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            CheckIfPlayerInFOV();
        }
    }

    void CheckIfPlayerInFOV()
    {
        Collider[] collisionChecks = Physics.OverlapSphere(projectileSpawnPoint.position, fovRadius, targetMask);
        
        if (collisionChecks.Length != 0)
        {
            Transform target = collisionChecks[0].transform;

            Vector3 directionToTarget = (target.position - projectileSpawnPoint.position).normalized;

            float distanceToTarget = Vector3.Distance(projectileSpawnPoint.position, target.position);

            if (!Physics.Raycast(projectileSpawnPoint.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                projectileSpawnPoint.right = directionToTarget;
                Projectile p = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                Debug.Log(p.transform.position);
            }
        }
    }

    public void TakeDamage(int damage, GameObject instigator)
    {
        // Destroy the enemy if it takes damage
        health -= damage;
        if (health <= 0)
        {
            if (instigator.GetComponent<Player>() != null)
            {
                instigator.GetComponent<Player>().AddXP(xpValue);
            }
            Destroy(gameObject);
        }
    }

}

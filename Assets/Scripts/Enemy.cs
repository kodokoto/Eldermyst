using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, TakeDamage
{

    public int health = 20;

    public float fovRadius = 10f;

    public Transform projectileSpawnPoint;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public Projectile projectile;

    public float fireRate = 2f;


    void Start()
    {
        StartCoroutine(Routine());
    }

    public IEnumerator Routine()
    {
        // while the enem
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            CheckIfPlayerInFOV();
        }
    }

    void CheckIfPlayerInFOV()
    {
        Collider2D[] collisionChecks = Physics2D.OverlapCircleAll(projectileSpawnPoint.position, fovRadius, targetMask);
        
        if (collisionChecks.Length != 0)
        {
            Transform target = collisionChecks[0].transform;

            Vector2 directionToTarget = (target.position - projectileSpawnPoint.position).normalized;

            float distanceToTarget = Vector2.Distance(projectileSpawnPoint.position, target.position);

            if (!Physics2D.Raycast(projectileSpawnPoint.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                // get rotation to face player
                float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
                projectileSpawnPoint.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Destroy the enemy if it takes damage
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}

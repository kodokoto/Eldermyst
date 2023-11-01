using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage
{

    public int health = 20;
    public int Xp;

    public float fovRadius = 10f;

    public Transform projectileSpawnPoint;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public Projectile projectile;

    public float fireRate = 2f;
    public GameObject playerObj;

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

    public void TakeDamage(int damage)
    {
        // Destroy the enemy if it takes damage
        health -= damage;
        if (health <= 0)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            Player player = (Player)playerObj.GetComponent(typeof(Player));
            Destroy(gameObject);
            player.GainXP(Xp);
        }
    }

}

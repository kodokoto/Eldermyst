using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage
{



    public Transform projectileSpawnPoint;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public Projectile projectile;

    public Player player;

    public int health = 20;
    public float fovRadius = 10f;
    private float fireRate = 0.2f;
    public int xpValue = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
        if (player.IsInvisible)
        {
            return;
        }
        
        Collider[] collisionChecks = Physics.OverlapSphere(projectileSpawnPoint.position, fovRadius, targetMask);
        
        if (collisionChecks.Length != 0)
        {
            Transform target = collisionChecks[0].transform;

            Vector3 directionToTarget = (target.position - projectileSpawnPoint.position).normalized;

            float distanceToTarget = Vector3.Distance(projectileSpawnPoint.position, target.position);

            if (!Physics.Raycast(projectileSpawnPoint.position, directionToTarget, distanceToTarget, obstructionMask))
            {
                Attack(directionToTarget);
            }
        }
    }

    private void Attack(Vector3 direction)
    {
        projectileSpawnPoint.right = direction;
        Projectile p = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Debug.Log(p.transform.position);
    }

    public void TakeDamage(int damage)
    {
        // Destroy the enemy if it takes damage
        health -= damage;
        if (health <= 0)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(playerObj);
            Player player = (Player)playerObj.GetComponent(typeof(Player));
            Debug.Log(player);
            player.AddXP(xpValue);
            Debug.Log("Enemy destroyed");
            
            Destroy(gameObject);
        }
    }

}

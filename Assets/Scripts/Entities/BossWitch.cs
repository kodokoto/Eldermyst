using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossWitch : Enemy, IPathable, IFiresProjectiles
{

    private Collider Col;
    public PathfinderGrid Grid { get; set;}
    public List<Vector3> CurrentPath { get; set; }

    protected override int XpValue { get; set; } = 10;

    [SerializeField] protected override int Health { get; set; } = 50;
    [SerializeField] protected override float SearchRange { get; set; } = 20f;
    [SerializeField] protected override float AttackRate { get; set; } = 1f;
    [SerializeField] protected override float AttackRange { get; set; } = 10f;
    [SerializeField] protected override int AttackDamage { get; set; } = 10;
    [field: SerializeField] public Transform ProjectileSpawnPoint { get; set; }
    public Projectile projectile;
    [SerializeField] protected float movementSpeed = 3f;

    [SerializeField] private Freeze IceSpell;
    private bool IceAttackReady = true;

    private bool ProjectileAttackReady = true;

    private bool warpIsAvailable = true;





    void Awake()
    {
        // cache grid
        Grid = GameObject.Find("A*").GetComponent<PathfinderGrid>();
        Col = GetComponent<Collider>();
    }

    protected override void Attack()
    {
        if (Player.IsGhost)
        {
            return;
        }
        Animator.SetTrigger("Attack");
        if (Vector3.Distance(transform.position, Player.GetComponent<Collider>().bounds.center) < IceSpell.radius && IceAttackReady)
        {
            StartCoroutine(IceAttack());
        }
        else if (ProjectileAttackReady)
        {
            ProjectileSpawnPoint.LookAt(Player.GetComponent<Collider>().bounds.center);
            Instantiate(projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
        }
    }

    private IEnumerator IceAttack()
    {
        IceAttackReady = false;
        
        IceSpell.Activate(gameObject);
        yield return new WaitForSeconds(3f);
        IceSpell.Deactivate(gameObject);


        // cooldown
        yield return new WaitForSeconds(5f);
        IceAttackReady = true;
    } 



    protected override void Chase()
    {
        // if the search rate has been reached, search for the player
        if (CanSearch)
        {
            Debug.Log("Searching...");
            // if the player is in line of sight
            if (PlayerInLOS())
            {

                // if the player is within attack range, attack
                if (Vector3.Distance(transform.position, Player.GetComponent<Collider>().bounds.center) < AttackRange)
                {
                    Debug.Log("Player in LOS, within attack range, attacking");
                    CurrentState = EnemyState.Attacking;
                    Animator.SetBool("Moving", false);
                } else if (warpIsAvailable)
                {
                    StartCoroutine(WarpTo(Player.GetComponent<Collider>().bounds.center, 5f));
                }
                    
            }
            else if (warpIsAvailable)
            {
                StartCoroutine(WarpTo(Grid.GetRandomWalkablePosition(), 3f));
            }
        }
    }

    public IEnumerator WarpTo(Vector3 position, float cooldown)
    {
        warpIsAvailable = false;
        Debug.Log("Warping");
        transform.position = position;
        yield return new WaitForSeconds(cooldown);
        warpIsAvailable = true;
    }

      // OnDrawGizmos
    // draw the last known player position
   public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(LastKnownPlayerPosition, 1f);

                // draw search range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, SearchRange);

        // draw attack range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, AttackRange);


        // draw the path
        if (CurrentPath != null)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 node in CurrentPath)
            {
                Gizmos.DrawWireSphere(node, 0.5f);
            }
        }
    }



}
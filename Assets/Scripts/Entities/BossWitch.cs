using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossWitch : Enemy, IPathable, IFiresProjectiles
{

    private Collider Col;
    public PathfinderGrid Grid { get; set;}
    public List<Vector3> CurrentPath { get; set; }

    protected override int XpValue { get; set; } = 10;

    [SerializeField] protected override int Health { get; set; } = 100;
    [SerializeField] protected override float SearchRange { get; set; } = 30f;
    [SerializeField] protected override float AttackRate { get; set; } = 1f;
    [SerializeField] protected override float AttackRange { get; set; } = 20f;
    [SerializeField] protected override int AttackDamage { get; set; } = 10;
    [field: SerializeField] public Transform ProjectileSpawnPoint { get; set; }
    public Projectile projectile;
    [SerializeField] protected float movementSpeed = 3f;

    [SerializeField] private Freeze IceSpell;
    private bool IceAttackReady = true;

    private bool ProjectileAttackReady = true;


    void Awake()
    {
        // cache grid
        Grid = GameObject.Find("A*").GetComponent<PathfinderGrid>();
        Col = GetComponent<Collider>();
    }

    protected override void Attack()
    {
        return; // DELETE THIS
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

    protected void Move()
    {
        // while we have a path, move along it
        if (CurrentPath != null ) {
            Debug.Log("Moving along path");
            if (CurrentPath.Count > 0) {
                Debug.Log("Moving along path 2");
                if (CurrentPath.Count > 1) {
                        // angle between current node and next node
                    float angle = Mathf.Rad2Deg * Mathf.Atan2(CurrentPath[1].y - CurrentPath[0].y, CurrentPath[1].x - CurrentPath[0].x);
                    Debug.Log("Angle between current node and next node: " + angle);
                    // if the angle is too steep, teleport to the next node
                    while (angle != 0f && angle != 180f && CurrentPath.Count > 1) {
                        transform.position = CurrentPath[1];
                        CurrentPath.RemoveAt(0);
                    }
                }
                Debug.Log("Moving to: " + CurrentPath[0]);
                Debug.Log("Distance to next node: " + Vector3.Distance(transform.position, CurrentPath[0]));
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(CurrentPath[0].x, transform.position.y), movementSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, CurrentPath[0]) < 0.5f) {
                    CurrentPath.RemoveAt(0);   
                }
            } else {
                CurrentPath = null;
            }
        }
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
                }
                // get the path to the player
                Debug.Log("Player in LOS, pathing to player");

                List<Vector3> newPath = Grid.GetPath(transform.position, Player.GetComponent<Collider>().bounds.center, PathfindingMode.Walking);

                if (newPath == null)
                {
                    Debug.Log("Something is wrong, Path is null");
                }
                else
                {
                    CurrentPath = newPath;
                }
                Move();
            }
            else
            {
                Debug.Log("Player not in LOS");
                // if we have a path, move along it
                if (CurrentPath != null)
                {
                    Debug.Log("Moving along path");
                    Move();
                }
                // if we don't have a path, and we are not near the last known player position, change state to alert
                else if (Vector3.Distance(transform.position, LastKnownPlayerPosition) < 1f)
                {
                    Debug.Log("Player not in LOS, near last known player position, changing state to alert");
                    CurrentState = EnemyState.Alert;
                }
                // else get the path to the last known player position
                else
                {
                    Debug.Log("Player not in LOS, pathing to last known player position");
                    CurrentPath = Grid.GetPath(transform.position, LastKnownPlayerPosition);
                }
            }
        }
        else
        {
            Move();
        }
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
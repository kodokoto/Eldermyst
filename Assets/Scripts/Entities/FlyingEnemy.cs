using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingEnemy : Enemy, IPathable
{

    private Collider Col;
    public PathfinderGrid Grid { get; set;}
    public List<Vector3> CurrentPath { get; set; }

    [field: SerializeField] protected override int Health { get; set; } = 50;
    [field: SerializeField] protected override int XpValue { get; set; } = 10;
    [field: SerializeField] protected override float SearchRange { get; set; } = 20f;
    [field: SerializeField] protected override float AttackRate { get; set; } = 1f;
    [field: SerializeField] protected override float AttackRange { get; set; } = 3f;
    [field: SerializeField] protected override int AttackDamage { get; set; } = 10;

    [SerializeField] protected float flyingSpeed = 5f;

    void Awake()
    {
        // cache grid
        Grid = GameObject.Find("A*").GetComponent<PathfinderGrid>();
        Col = GetComponent<Collider>();
    }

    protected override void Attack()
    {
        Player.TakeDamage(AttackDamage);
        Animator.SetTrigger("Attack");
    }

    protected void Move()
    {
        // while we have a path, move along it
        if (CurrentPath != null) {
            if (CurrentPath.Count > 0) {
                if (CurrentPath[0] != Vector3.zero) {
                    Quaternion targetRotation = Quaternion.LookRotation(CurrentPath[0] - transform.position).normalized;
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                }
                Vector3 direction = CurrentPath[0];
                // direction.y += Random.Range(-0.5f, 0.5f); // add some randomness to the y axis for a more natural flight path
                transform.position = Vector3.MoveTowards(transform.position, direction, flyingSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, direction) < 0.1f) {
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
            // if the player is in line of sight
            if (PlayerInLOS())
            {

                // if the player is within attack range, attack
                if (Vector3.Distance(transform.position, Player.GetComponent<Collider>().bounds.center) < AttackRange)
                {
                    CurrentState = EnemyState.Attacking;
                }
                // get the path to the player

                List<Vector3> newPath = Grid.GetPath(transform.position, Player.GetComponent<Collider>().bounds.center);

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
                // if we have a path, move along it
                if (CurrentPath != null)
                {
                    Move();
                }
                // if we don't have a path, and we are not near the last known player position, change state to alert
                else if (Vector3.Distance(transform.position, LastKnownPlayerPosition) < 1f)
                {
                    CurrentState = EnemyState.Alert;
                }
                // else get the path to the last known player position
                else
                {
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
        Gizmos.DrawWireSphere(this.transform.localPosition, SearchRange);

        // draw attack range
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(this.transform.position, AttackRange);

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
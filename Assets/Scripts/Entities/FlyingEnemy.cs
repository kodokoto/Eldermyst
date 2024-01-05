using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingEnemy : Enemy, IPathable
{

    private Collider Col;
    public PathfinderGrid Grid { get; set;}
    public List<Vector3> CurrentPath { get; set; }

    protected override int XpValue { get; set; } = 10;
    protected override float SearchRange { get; set; } = 20f;
    protected override float AttackRange { get; set; } = 1f;
    protected float flyingSpeed = 8f;
    private float searchRate = 0.1f;
    private float searchTimer = 0f;


    void Awake()
    {
        // cache grid
        Grid = GameObject.Find("A*").GetComponent<PathfinderGrid>();
        Col = GetComponent<Collider>();
    }

    protected override void Attack()
    {
        return;
    }

    protected void Move()
    {
        // while we have a path, move along it
        if (CurrentPath != null ) {
            Debug.Log("Moving along path");
            if (CurrentPath.Count > 0) {
                Debug.Log("Moving along path 2");
                transform.position = Vector3.MoveTowards(transform.position, CurrentPath[0], flyingSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, CurrentPath[0]) < 0.1f) {
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
        if (searchRate < searchTimer)
        {
            Debug.Log("Searching...");
            searchTimer = 0f;
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
            searchTimer += Time.deltaTime;
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
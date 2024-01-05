using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Enemy : MonoBehaviour, ITakeDamage, IFreezable
{

    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected LayerMask obstructionMask = LayerMask.GetMask("Ground");
    protected Player Player { get; private set; }
    protected abstract int XpValue { get; set; }
    public bool IsFrozen { get; set; }
    protected int Health { get; set; }
    protected int SearchRate { get; } 
    protected abstract float SearchRange { get; set; }
    protected abstract float AttackRange { get; set; }

    protected enum EnemyState
    {
        Alert,
        Chasing,
        Attacking,
    }

    protected EnemyState CurrentState { get; set; }

    protected Vector3 LastKnownPlayerPosition { get; set; }

    void Awake()
    {
        // cache player
    }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        obstructionMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("StickyWall");
    }

    void Update()
    {
        // Debug.Log("Current State: " + CurrentState);
        switch (CurrentState)
        {
            case EnemyState.Alert:
                OnAlert();
                break;
            case EnemyState.Chasing:
                OnChase();
                break;
            case EnemyState.Attacking:
                OnAttack();
                break;
        }
        Debug.Log("Current State: " + CurrentState);
    }

    protected virtual void OnAlert()
    {
        // every SearchRate seconds, call SearchForPlayer()
        if (PlayerInLOS())
        {
            // if player is found, set state to chasing
            CurrentState = EnemyState.Chasing;
        }
    }

    protected virtual void OnChase()
    {
        // search for player 
        // Debug.Log("OnChase");
        if (Vector3.Distance(transform.position, Player.transform.position) <= AttackRange)
        {
            // Debug.Log("Player In Attack Range");
            CurrentState = EnemyState.Attacking;
            Attack(); 
        }
        else
        {
            Chase();
        }
    }

    protected virtual void OnAttack()
    {
        // attack player
        // if player is not in attack range, chase
        if (Vector3.Distance(transform.position, Player.transform.position) <= AttackRange)
        {
            Attack();
        }
        else
        {
            CurrentState = EnemyState.Chasing;
        }
    }

    protected abstract void Attack();

    protected abstract void Chase();
    
    protected bool PlayerInLOS()
    {
        // Debug.Log("Checking for Player in LOS");

        Vector3 playerPosition = Player.GetComponent<Collider>().bounds.center;

        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
        // if player is within search range, check for line of sight
        if (distanceToPlayer <= SearchRange)
        {
            // get direction to player
            Vector3 directionToTarget = (playerPosition - transform.position).normalized;

            // draw raycast to player
            // Debug.DrawRay(transform.position, directionToTarget * distanceToPlayer, Color.red);

            // raycast to player by FOV radius
            Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToPlayer, obstructionMask);

            // Debug.Log("Raycast Hit: " + hit.collider);
            // if player is found, set state to chasing
            if (hit.collider == null)
            {
                LastKnownPlayerPosition = playerPosition;
                // draw line to player
                Debug.DrawLine(transform.position, playerPosition, Color.red);
                return true;
            }

        }

        return false;

    }

    public void Freeze(int damage)
    {
        IsFrozen = true;
        TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        // Destroy the enemy if it takes damage
        Health -= damage;
        if (Health <= 0)
        {
            Player.AddXP(XpValue);
            Destroy(gameObject);
        }
    }

    
}

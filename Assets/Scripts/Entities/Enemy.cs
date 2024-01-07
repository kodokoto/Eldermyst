using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public abstract class Enemy : MonoBehaviour, ITakeDamage, IFreezable
{

    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected LayerMask obstructionMask;

    protected Animator Animator;
    protected Player Player { get; private set; }
    protected abstract int XpValue { get; set; }
    public bool IsFrozen { get; set; }
    protected abstract int Health { get; set; }
    protected float SearchRate { get; } = 0.1f;
    protected abstract float SearchRange { get; set; }
    protected abstract float AttackRate { get; set; }
    protected abstract float AttackRange { get; set; }
    protected abstract int AttackDamage { get; set; }

    private float searchTimer = 0f;
    private float attackTimer = 0f;

    protected bool CanSearch { get {return searchTimer >= SearchRate; } }
    protected bool CanAttack { get {return attackTimer >= AttackRate; } }
    

    protected enum EnemyState
    {
        Alert,
        Chasing,
        Attacking,
    }

    protected EnemyState CurrentState { get; set; }

    protected Vector3 LastKnownPlayerPosition { get; set; }

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        obstructionMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("StickyWall");
        Animator = GetComponent<Animator>();
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
        HandleTimers();
    }

    private void HandleTimers()
    {
        if (CanAttack)
        {
            attackTimer = 0f;
        }
        if (CanSearch)
        {
            searchTimer = 0f;
        }
        searchTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;
    }

    protected virtual void OnAlert()
    {
        // every SearchRate seconds, call SearchForPlayer()
        if (PlayerInLOS())
        {
            // if player is found, set state to chasing
            Debug.Log("Player found, chasing");
            CurrentState = EnemyState.Chasing;
        }
    }

    protected virtual void OnChase()
    {
        // search for player 
        if (Vector3.Distance(transform.position, Player.transform.position) <= AttackRange)
        {
            Debug.Log("Player in attack range, attacking");
            CurrentState = EnemyState.Attacking;
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
        if (Vector3.Distance(transform.position, Player.GetComponent<Collider>().bounds.center) <= AttackRange)
        {
            if (CanAttack)
            {
                Debug.Log("Attacking");
                Attack();
            }
        }
        else
        {
            Debug.Log("Player not in attack range, chasing");
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
            Animator.SetTrigger("Death");
            Destroy(gameObject);
        }
        Animator.SetTrigger("Hit");
    }
}

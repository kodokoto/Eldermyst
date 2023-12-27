using System;
using System.Collections;
using UnityEngine;


public abstract class Enemy : MonoBehaviour, ITakeDamage, IFreezable
{

    [SerializeField] protected LayerMask targetMask;
    [SerializeField] protected LayerMask obstructionMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("StickyWall");
    protected Player Player { get; private set; }
    private int XpValue { get; set; }
    public bool IsFrozen { get; set; }
    protected int Health { get; set; }
    protected int SearchRate { get; }
    protected float SearchRange { get; set; }
    protected float AttackRange { get; set; }

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
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        obstructionMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("StickyWall");
    }

    void Update()
    {
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
        if (PlayerInLOS())
        {
            // if player is within attack range, set state to attacking
            if (Vector3.Distance(transform.position, Player.transform.position) <= AttackRange)
            {
                CurrentState = EnemyState.Attacking;
            }
            // else move towards player
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Time.deltaTime * 5f);
            }
        }
        else
        {
            // if player is not found, move towards last known position
            transform.position = Vector3.MoveTowards(transform.position, LastKnownPlayerPosition, Time.deltaTime * 5f);
        }
    }

    protected abstract void OnAttack();
    
    private bool PlayerInLOS()
    {
        // get direction to player
        Vector3 directionToTarget = (Player.transform.position - transform.position).normalized;

        // raycast to player by FOV radius
        Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, SearchRange, obstructionMask);

        // if player is found, set state to chasing
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            LastKnownPlayerPosition = Player.transform.position;
            return true;
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

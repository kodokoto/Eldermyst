using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class DarkOrb : MonoBehaviour, IAutoMove, IFreezable
{
    // Start is called before the first frame update

    [SerializeField] public AutoMoveType AutoMoveType { get; set; }
    public Vector3[] Path { get; set; }
    public int TargetPointIndex { get; set; }
    public bool IsMoving { get; set; }
    public float Speed { get; set; }
    public bool IsFrozen { get; set; } 

    public int Damage;
    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, Path[TargetPointIndex], Time.deltaTime * Speed);
    }

    void Awake()
    {
        Path = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Path[i] = transform.GetChild(i).position;
        }
        ((IAutoMove)this).CheckPath();
        transform.position = Path[0];

    }


    void Start()
    {
        AutoMoveType = AutoMoveType.PingPong;
        Speed = 5f;
        ((IAutoMove)this).AdvanceTarget();
        IsMoving = true;
        IsFrozen = false;
    }

    void Update()
    {
        if (IsMoving && !IsFrozen)
        {
            if (transform.position == ((IAutoMove)this).GetCurrentPoint())
            {
                ((IAutoMove)this).AdvanceTarget();
            }
            else
            {
                MoveToTarget(); 
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.ConsumeMana(Damage);
            player.TakeDamage(Damage);
        }
    }

}

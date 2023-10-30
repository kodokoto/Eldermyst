using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public enum AutoMoveType
{
    Loop,
    PingPong,
    OneWay
}

public interface IAutoMove
{

    AutoMoveType AutoMoveType { get; set; }
    bool IsMoving { get; set; }
    Vector3[] Path { get; set; }
    float Speed { get; set; }

    int TargetPointIndex { get; set; }

    Vector3 GetCurrentPoint()
    {
        return Path[TargetPointIndex];
    }

    int AdvanceTarget()
    {
        if (IsMoving)
        {
            switch (AutoMoveType)
            {
                case AutoMoveType.Loop:
                    AdvanceTargetLooping();
                    break;
                case AutoMoveType.PingPong:
                    AdvanceTargetPingPong();
                    break;
                case AutoMoveType.OneWay:
                    AdvanceTargetOneWay();
                    break;
                default:
                    break;
            }
        }
        return TargetPointIndex;
    }

    void AdvanceTargetLooping()
    {
        if (TargetPointIndex == Path.Length - 1)
        {
            TargetPointIndex = 0;
        }
        else
        {
            TargetPointIndex++;
        }
    }

    void AdvanceTargetPingPong()
    {
        if (TargetPointIndex == Path.Length - 1)
        {
            IsMoving = false;
        }
        else
        {
            TargetPointIndex++;
        }
    }

    void AdvanceTargetOneWay()
    {
        if (TargetPointIndex == Path.Length - 1)
        {
            TargetPointIndex = 0;
        }
        else
        {
            TargetPointIndex++;
        }
    }

    void MoveToNextPoint(){}

    void CheckPath()
    {
        // use SphereCast between each point in the path to make sure there is a clear path between them
        // if there is not a clear path, log an error and disable the platform

        for (int i = 0; i < Path.Length - 1; i++)
        {
            Vector3 direction = Path[i + 1] - Path[i];
            float distance = Vector3.Distance(Path[i + 1], Path[i]);
            RaycastHit hit;
            if (Physics.SphereCast(Path[i], 0.5f, direction, out hit, distance, LayerMask.GetMask("StickyWall")))
            {
                Debug.LogError("Path is blocked by " + hit.collider.gameObject.name);
                IsMoving = false;
            }
        }
    }

    void DrawGizmos(Color c)
    {
        if (Path != null && Path.Length > 0)
        {
            Debug.Log("Drawing Gizmos");
            Debug.Log(Path.Length);
            Gizmos.color = c;
            Gizmos.DrawLineList(Path);
            if (AutoMoveType == AutoMoveType.Loop)
            {
                Gizmos.DrawLine(Path[0], Path[^1]);
            }
        }
    }

}

public class MovingPlatform : Platform, IAutoMove
{
    [SerializeField] public AutoMoveType AutoMoveType { get; set; }
    public Vector3[] Path { get; set;}
    public int TargetPointIndex { get; set; }
    public bool IsMoving { get; set; }
    public Color color;
    public float Speed { get; set; }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, Path[TargetPointIndex], Time.deltaTime * Speed);
    }

    void Awake()
    {
        Debug.Log("Awake");
        gameObject.layer = LayerMask.NameToLayer("Ground");
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
        AutoMoveType = AutoMoveType.Loop;
        Speed = 5f;
        ((IAutoMove)this).AdvanceTarget();
        IsMoving = true;
    }

    void Update()
    {
        if (IsMoving)
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

    void OnDrawGizmos()
    {
        ((IAutoMove)this).DrawGizmos(color);
    }
}

using System.Collections.Generic;
using UnityEngine;

public interface IPathable
{
    PathfinderGrid Grid { get; set;}
    List<Vector3> CurrentPath { get; set; }
}
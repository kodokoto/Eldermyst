using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Teleport : Spell
{
    public float teleportDistance;
    public Vector2 preCalcPosition;

    public override bool IsValid(GameObject parent)
    {
        Debug.Log("Checking if valid");
        //  use overlapsphere in the direction the player is facing
        //  if the spherecast line hits a wall but it ends not colliding with one, return true
        //  else return false

        Vector3 direction = parent.transform.right;
        Vector3 origin = parent.transform.position;
        Vector3 destination = origin + direction * teleportDistance;

        Collider[] intersecting = Physics.OverlapSphere(destination, parent.transform.localScale.y / 2f, LayerMask.GetMask("StickyWall"));
        
        if (intersecting.Length > 0)
        {
            Debug.Log("Not valid");
            return false;
        }
        
        return true;
    }

    public override void Activate(GameObject parent)
    {
        Vector3 direction = parent.transform.right;
        Vector3 origin = parent.transform.position;
        Vector3 destination = origin + direction * teleportDistance;
        parent.transform.position = destination;
    }
}

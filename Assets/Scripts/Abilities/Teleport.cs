using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Teleport : Spell
{
    public float teleportDistance;

    public Vector2 preCalcPosition;


    public override bool IsValid(GameObject parent)
    {
        Debug.Log("Checking if valid");
        //  raycast in the direction the player is facing
        RaycastHit hit;
        if (!Physics.Raycast(parent.transform.position, parent.transform.right, out hit, teleportDistance, LayerMask.GetMask("StickyWall", "Ground")))
        {
                Debug.Log("Ladies and gentlemen, we got him");
            // if the raycast hits something, check if it's a valid teleport location
                return true;
        }
        return false;
    }

    public override void Activate(GameObject parent)
    {
        Debug.Log("Teleporting");
        // set the player's position to the new position
        parent.transform.position = preCalcPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Teleport : Spell
{
    [SerializeField] public new int manaCost = 10;

    public float teleportDistance;

    public Vector2 preCalcPosition;

    public override bool isValid(GameObject parent)
    {

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // get the direction from the player to the mouse
        Vector2 parentPosition = parent.transform.position;
        Vector2 direction = mousePosition - parentPosition;
        // normalize the direction vector
        direction.Normalize();
        // multiply the direction vector by the teleportDistance
        direction *= teleportDistance;
        // add the direction vector to the player's position
        Vector2 newPosition = parentPosition + direction;

        LayerMask solidLayers = LayerMask.GetMask("Ground") | LayerMask.GetMask("StickyWall");

        if (Physics2D.OverlapCircle(newPosition, 0.5f, solidLayers))
        {
            Debug.Log("Teleport is invalid");
            return false;
        }
        else 
        {
            preCalcPosition = newPosition;
            Debug.Log("Teleport is Valid");
            return true;
        }
    }

    public override void Activate(GameObject parent)
    {
        // set the player's position to the new position
        Debug.Log("Teleport is happening");
        parent.transform.position = preCalcPosition;
    }
}

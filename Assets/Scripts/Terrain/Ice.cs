using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
            movement.runSpeed -= 2;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
            movement.runSpeed += 2;
        }
    }

}

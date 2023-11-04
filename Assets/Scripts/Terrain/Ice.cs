using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{

    public int slowDownAmount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
            movement.SetCurrentSpeed(movement.GetCurrentSpeed() - slowDownAmount);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
            movement.SetCurrentSpeed(movement.GetCurrentSpeed() + slowDownAmount);
        }
    }

}
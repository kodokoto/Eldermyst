using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lava : MonoBehaviour
{
    public int amount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.TakeDamage(amount);
           
        }
    }
}
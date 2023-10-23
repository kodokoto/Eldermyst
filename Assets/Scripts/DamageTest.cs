using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            collision.gameObject.GetComponent<Player>().TakeDamage(50);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player hit");
            collision.gameObject.GetComponent<Player>().takeDamage(50);
        }
    }
}

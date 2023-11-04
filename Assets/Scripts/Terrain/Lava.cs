using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lava : MonoBehaviour
{
    public int damage;
    public float damageRate;
    public float damageTimer;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.TakeDamage(damage, this.gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (damageTimer > damageRate)
            {
                Player player = other.gameObject.GetComponent<Player>();
                player.TakeDamage(damage, this.gameObject);
                damageTimer = 0;
            }
            else
            {
                damageTimer += Time.deltaTime;
            }
        }
    }
}
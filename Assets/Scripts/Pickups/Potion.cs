using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public enum PotionType
    {
        Health,
        Mana
    }

    public PotionType type;
    public int amount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (type == PotionType.Health)
            {
                player.restoreHealth(amount);
            }
            else if (type == PotionType.Mana)
            {
                player.restoreMana(amount);
            }
            Destroy(gameObject);
        }
    }
}

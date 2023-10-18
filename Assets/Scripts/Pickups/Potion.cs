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
            switch (type)
            {
                case PotionType.Health:
                    player.restoreHealth(amount);
                    break;
                case PotionType.Mana:
                    player.restoreMana(amount);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

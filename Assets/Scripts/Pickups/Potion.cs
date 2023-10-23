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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            switch (type)
            {
                case PotionType.Health:
                    player.RestoreHealth(amount);
                    break;
                case PotionType.Mana:
                    player.RestoreMana(amount);
                    break;
            }
            Destroy(gameObject);
        }
    }
}

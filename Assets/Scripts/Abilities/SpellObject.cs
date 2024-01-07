using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellObject : MonoBehaviour
{ 
    public string spellName;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.AquireSpells(spellName);
            Debug.Log($"{spellName} aquired");
            Destroy(gameObject);
        }
    }
}

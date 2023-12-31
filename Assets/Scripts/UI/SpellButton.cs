using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButton : MonoBehaviour
{

    public string SpellName;
    public Player player;
    SpellHandler spell;

    public void DealWithClick()
    {
        Debug.Log($"Spell clicked: {SpellName}");
        spell = player.searchSpells(SpellName);
        if (spell != null)
        {
            Debug.Log($"{SpellName} found");
            if (spell.isAquired == false)
            {
                Debug.Log($"You have not aquired {SpellName} yet");
                return;
            }
            if (spell.enabled == true)
            {
                if (spell.getKey() == KeyCode.Mouse0)
                {
                    player.isLeftEnabled = false;
                }
                else
                {
                    player.isRightEnabled = false;
                }
                spell.Lock();
                Debug.Log($"Lock {SpellName}");
                return;
            }
            else if (player.isRightEnabled && player.isLeftEnabled)
            {
                Debug.Log("Already have two spells. Unclick one to pick another spell");
                return;
            }
            spell.Unlock();
            if (player.isLeftEnabled || (!player.isLeftEnabled && !player.isRightEnabled))
            {
                spell.setKey(KeyCode.Mouse1);
                player.isRightEnabled = true;
            }
            else
            {
                spell.setKey(KeyCode.Mouse0);
                player.isLeftEnabled = true;
            }
            Debug.Log($"{SpellName} Unlocked with keyCode {spell.getKey()}");
        }
        
    }
}

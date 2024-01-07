using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCircle : MonoBehaviour
{
    public GameObject circle;
    public Player player;
    SpellHandler spell;
    public GameObject[] Buttons;
    public bool isActive = false;
    void Awake()
    {
        circle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            circle.SetActive(true);
            isActive = true;
            for (int i =0; i<Buttons.Length; i++)
            {
                SpellButton spells = Buttons[i].GetComponent<SpellButton>();
                string SpellName = spells.getName();
                spell = player.searchSpells(SpellName);
                if (spell.enabled==true)
                {
                    spells.ActivateButton();
                }
            }
        }
        else
        {
            isActive = false;
            circle.SetActive(false);
        }
    }

}

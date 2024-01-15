using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookManager : MonoBehaviour
{
    public GameObject[] spellSlots;
    public GameObject objectToInstantiate;
    [SerializeField] private SpellSignalSO spellSignal; // reference the spell signal
    private int Index = 0;

    public void OnEnable()
    {
        spellSignal.OnTriggered += NewSpell; // <- subscribe to the OnTrigger action
    }
    public void OnDisable()
    {
        spellSignal.OnTriggered -= NewSpell; // <- unsubscribe to the OnTrigger action (to prevent memory leaks)
    }


    // this function gets called when the action is triggered
    public void NewSpell(SpellHandler spellHandler)
    {
        addSpellToBook(spellHandler);
        Debug.Log("Spell added to book");
    }

    private void addSpellToBook(SpellHandler spellHandler)
    {
        // Here you have to code to add the spell to the book
        objectToInstantiate.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = spellHandler.Spell.spellName;
        string comboString = "";
        comboString = ComboString(spellHandler.Spell, comboString);
        
        objectToInstantiate.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = comboString;
        objectToInstantiate.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = spellHandler.Spell.icon;
        GameObject slot = Instantiate(objectToInstantiate, spellSlots[Index].transform);
        slot.transform.localPosition = new Vector3(-44, -475, 0);
        Index++;
    }

    private string ComboString(Spell spell, string comboString)
    {
        foreach (SpellCombo combo in spell.combos)
        {
            if (comboString == "")
            {
                comboString += combo.keys
                            .Select(item => (char)item)
                            .Aggregate(new StringBuilder(), (current, next) => current.Append(next))
                            .ToString();
            }
            else
            {
                comboString += " or " + combo.keys
                            .Select(item => (char)item)
                            .Aggregate(new StringBuilder(), (current, next) => current.Append(next))
                            .ToString();
            }

        }
        return comboString;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookManager : MonoBehaviour
{
    public GameObject[] spellSlots;
    public GameObject objectToInstantiate;
    public Spell PSpell;
    private int Index=0;
    [SerializeField] private SpellSignalSO spellSignal; // reference the spell signal
    Player player;
    List<Spell> spells;

    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        spells = player.PlayerInventory.Spells;
        Debug.Log("Start book manager");
        for(int i = 0; i < spells.Count; i++)
        {
            addSpellToBook(spells[i]);
        }
        Debug.Log("Book UI should be instantiated");
    }

    public void OnEnable()
    {
        spellSignal.OnTriggered += NewSpell; // <- subscribe to the OnTrigger action
    }
    public void OnDisable()
    {
        spellSignal.OnTriggered -= NewSpell; // <- unsubscribe to the OnTrigger action (to prevent memory leaks)
    }


    // this function gets called when the action is triggered
    public void NewSpell(Spell spell)
    {
        addSpellToBook(spell);
        Debug.Log("Spell added to book");
    }

    private void addSpellToBook(Spell spell)
    {
        // Here you have to code to add the spell to the book
        objectToInstantiate.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = spell.spellName;
        string comboString = "";
        comboString = ComboString(spell, comboString);
        
        objectToInstantiate.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = comboString;
        objectToInstantiate.transform.GetChild(2).gameObject.GetComponent<Image>().sprite = spell.icon;
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
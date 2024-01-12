using System.Collections;
using System.Collections.Generic;
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

    public void Start()
    {
        Debug.Log("Start book manager");
        //objectToInstantiate.transform.GetChild(0).GetComponent<TextMeshPro>().text = PSpell.spellName;
        objectToInstantiate.transform.GetChild(2).GetComponent<Image>().sprite = PSpell.icon;
        GameObject slot =Instantiate(objectToInstantiate, spellSlots[Index].transform);
        slot.transform.localPosition = new Vector3(-44, -475, 0);
        Index++;
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
        // Here you have to code to add the spell to the book
        objectToInstantiate.transform.GetChild(0).GetComponent<TextMeshPro>().text = spell.name ;

        //objectToInstantiate.transform.GetChild(1).GetComponent<TextMeshPro>().text = spell.Spell.combo;
        objectToInstantiate.transform.GetChild(2).GetComponent<Image>().sprite = spell.icon;
        Instantiate(objectToInstantiate, spellSlots[Index].transform);
        Index++;
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellIcons : MonoBehaviour
{
    // Start is called before the first frame update

    public Player player;
    public List<SpellHandler> Spells;
    public GameObject ObjectToInstantiate;
    public List<GameObject> ObjectsInstantiated = new List<GameObject>();

    [SerializeField] private SpellSignalSO _onSpellAqured;
    private int index = 0;

    private void OnEnable()
    {
        _onSpellAqured.OnTriggered += OnNewSpellAcquired;
    }

    private void OnDisable()
    {
        _onSpellAqured.OnTriggered -= OnNewSpellAcquired;
    }

    void Start()
    {
        Spells = player.SpellHandlers;
        for (int i = 0; i < Spells.Count; i++)
        {
           InstantiateSpellPrefab(Spells[i].Spell);
        }

        RearrangePositions();
    }

    private void OnNewSpellAcquired(Spell spell)
    {
        InstantiateSpellPrefab(spell);
        RearrangePositions();
    }

    private void RearrangePositions()
    {
        // Rearrange positions so that the first spell is furthest left
        // and the last spell is furthest right
        // iterate from reverse
        for (int i = ObjectsInstantiated.Count - 1; i >= 0; i--)
        {
            ObjectsInstantiated[i].transform.localPosition = new Vector3(i * 40, 0, 0);
        }
    }
    private void InstantiateSpellPrefab(Spell spell)
    {
        // instantiate prefab
        GameObject spellPrefab = Instantiate(ObjectToInstantiate);
        // set image
        spellPrefab.GetComponent<Image>().sprite = spell.icon;

        // set parent
        spellPrefab.transform.SetParent(gameObject.transform);

        // set anchor to top right
        spellPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);

        ObjectsInstantiated.Add(spellPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        deaLWithCooldown();
    }

    private void InstantiatePrefabs()
    {
        for (int i = index; i < Spells.Count; i++)
        {
            Spells = player.SpellHandlers;
            Debug.Log("Spells found in SpellIcons");
            Image[] img = ObjectToInstantiate.GetComponentsInChildren<Image>();
            Transform child2 = ObjectToInstantiate.transform.GetChild(2);
            child2.gameObject.GetComponent<Image>().sprite = Spells[i].Spell.icon;
            Transform child = ObjectToInstantiate.transform.GetChild(0);
            Slider slider = child.gameObject.GetComponent<Slider>();
            slider.maxValue = Spells[i].Spell.cooldownTime;
            ObjectsInstantiated[index] = Instantiate(ObjectToInstantiate);
            ObjectsInstantiated[index].transform.parent = gameObject.transform;
            if (index > 0)
            {
                ObjectsInstantiated[index].transform.localPosition = new Vector3(ObjectsInstantiated[index - 1].transform.localPosition.x + 40, ObjectsInstantiated[index - 1].transform.localPosition.y, ObjectsInstantiated[index - 1].transform.localPosition.z);
            }
            else
            {
                ObjectsInstantiated[index].transform.localPosition = new Vector3(0, 0, 0);
            }
            index++;
            Debug.Log("Image should be instantiated");
        }
    }

    private void deaLWithCooldown()
    {
        Spells = player.SpellHandlers;
        for (int i = 0; i < Spells.Count; i++)
        {
            if (Spells[i].GetState() == SpellState.Cooldown)
            {
                StartCoroutine(changeSlider(ObjectsInstantiated[i]));
            }
        }
    }

    private IEnumerator changeSlider(GameObject obj)
    {
        Transform child = obj.transform.GetChild(0);
        Slider slider = child.gameObject.GetComponent<Slider>();
        for (int i=0; i<= slider.maxValue; i++)
        {
            slider.value = i;
            yield return new WaitForSeconds(1);
        }

    }
}

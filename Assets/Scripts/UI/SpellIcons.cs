using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellIcons : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ObjectToInstantiate;
    private readonly List<Tuple<SpellHandler, GameObject>> _cachedSpells = new List<Tuple<SpellHandler, GameObject>>();
    [SerializeField] private SpellSignalSO _onSpellAqured;

    private void OnEnable()
    {
        _onSpellAqured.OnTriggered += OnNewSpellAcquired;
    }

    private void OnDisable()
    {
        _onSpellAqured.OnTriggered -= OnNewSpellAcquired;
    }

    private void OnNewSpellAcquired(SpellHandler spellHandler)
    {
        InstantiateSpellPrefab(spellHandler);
        RearrangePositions();
    }

    private void RearrangePositions()
    {
        // Rearrange positions so that the first spell is furthest left
        // and the last spell is furthest right
        // iterate from reverse
        for (int i = 0; i < _cachedSpells.Count; i++)
        {
            _cachedSpells[i].Item2.transform.localPosition = new Vector3(i * -40, 0, 0);
        }

    }
    private void InstantiateSpellPrefab(SpellHandler spellHandler)
    {
        // instantiate prefab
        GameObject spellPrefab = Instantiate(ObjectToInstantiate);

        Debug.Assert(spellPrefab != null, "spellPrefab did not instantiate properly");
        // set image
        spellPrefab.GetComponentsInChildren<Image>().Where(item => item.name == "Icon").ToList()[0].sprite = spellHandler.Spell.icon;
        // set parent
        spellPrefab.transform.SetParent(gameObject.transform);

        // set anchor to top right
        spellPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);

        _cachedSpells.Add(new Tuple<SpellHandler, GameObject>(spellHandler, spellPrefab));
    }

    // Update is called once per frame
    void Update()
    {
        HandleCooldown();
    }

    private void HandleCooldown()
    {
        if (_cachedSpells == null) return;
        foreach (Tuple<SpellHandler, GameObject> pair in _cachedSpells)
        {
            if (pair.Item1.GetState() == SpellState.Cooldown)
            {
                Slider slider = pair.Item2.GetComponentInChildren<Slider>();
                slider.value = 1 - pair.Item1.CooldownTimer / pair.Item1.Spell.cooldownTime;
            }
            else if (pair.Item1.GetState() == SpellState.Active)
            {
                // set slider to 0
                pair.Item2.GetComponentInChildren<Slider>().value = 0;
            }
        }
    }
}

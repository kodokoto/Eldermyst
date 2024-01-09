using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellSystem : MonoBehaviour
{
    private readonly Queue<KeyValuePair<SpellKey, float>> inputQueue = new();
    private readonly float maxComboDelay = 1f;
    [SerializeField] private InputManager inputManager = default;

    // Define combos
    private Player player;

    void Awake()
    {
        Debug.Assert(inputManager != null, "InputManager is null in SpellSystem, please assign it in the inspector");
    }

    void OnEnable()
    {
        inputManager.SpellEvent += OnSpellInput;
    }

    void OnDisable()
    {
        inputManager.SpellEvent -= OnSpellInput;
    }

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        ExpireOldInputs();
    }

    private void OnSpellInput(SpellKey key)
    {
        inputQueue.Enqueue(new KeyValuePair<SpellKey, float>(key, Time.time));
        CheckCombos();
    }

    private void ExpireOldInputs()
    {
        while (inputQueue.Count > 0 && Time.time - inputQueue.Peek().Value > maxComboDelay)
        {
            inputQueue.Dequeue();
        }
    }

    private void CheckCombos()
    {
        if (player?.SpellHandlers != null)
        {
            foreach (SpellHandler sh in player.SpellHandlers)
            {
                foreach (SpellCombo combo in sh.Spell.combos)
                {
                    if (IsComboMatch(combo.keys))
                    {
                        sh.Cast();
                        inputQueue.Clear();
                        break; // Consider if you want to break here or allow overlapping combos
                    }
                }
            }
        }
    }

    private bool IsComboMatch(List<SpellKey> combo)
    {
        if (inputQueue.Count < combo.Count) return false;

        var comboQueue = new Queue<SpellKey>(inputQueue.Select(kvp => kvp.Key));
        foreach (var key in combo)
        {
            if (comboQueue.Count == 0 || key != comboQueue.Dequeue())
                return false;
        }

        return true;
    }
}

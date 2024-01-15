using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpellSystem : MonoBehaviour
{
    private readonly List<KeyValuePair<SpellKey, float>> inputList = new();
    private readonly float maxComboDelay = 1f;
    [SerializeField] private InputManager inputManager = default;
    [SerializeField] private AudioSignalSO _sfxAudioSignal = default;

    [SerializeField] private AudioClip _combo1SFX = default;
    [SerializeField] private AudioClip _combo2SFX = default;
    [SerializeField] private AudioClip _combo3SFX = default;
    [SerializeField] private AudioClip _invalidComboSFX = default;

    private int validInputsCount = 0;
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
        inputList.Add(new KeyValuePair<SpellKey, float>(key, Time.time));
        ExpireOldInputs();
        CheckCombos();
    }

    private void ExpireOldInputs()
    {
        inputList.RemoveAll(kvp => Time.time - kvp.Value > maxComboDelay);
        if (inputList.Count == 0)
        {
            validInputsCount = 0;
        }
    }

    private void PlayComboSound(bool isComboInProgress)
    {
        AudioClip clipToPlay;
        if (isComboInProgress)
        {
            switch (validInputsCount % 3)
            {
                case 1: clipToPlay = _combo1SFX; break;
                case 2: clipToPlay = _combo2SFX; break;
                default: clipToPlay = _combo3SFX; break;
            }
        }
        else
        {
            clipToPlay = _invalidComboSFX;
            validInputsCount = 0; // Reset valid inputs count on invalid combo
        }

        _sfxAudioSignal.Trigger(clipToPlay, player.transform.position, 20f);
    }
 private void CheckCombos()
{
    bool isComboComplete = false;
    bool isPartialMatch = false;

    if (player?.SpellHandlers != null)
    {
        foreach (SpellHandler sh in player.SpellHandlers)
        {
            foreach (SpellCombo combo in sh.Spell.combos)
            {
                if (IsComboMatch(combo.keys, inputList))
                {
                    sh.Cast();
                    isComboComplete = true;
                    inputList.Clear();
                    break;
                }
                else if (IsPartialComboMatch(combo.keys, inputList))
                {
                    isPartialMatch = true;
                }
            }

            if (isComboComplete) break;
        }
    }

    if (isComboComplete)
    {
        validInputsCount = 0;
        PlayComboSound(true);
    }
    else if (isPartialMatch)
    {
        validInputsCount = (validInputsCount + 1) % 3;
        PlayComboSound(true);
    }
    else
    {
        validInputsCount = 0;
        PlayComboSound(false);
    }
}

private bool IsComboMatch(List<SpellKey> combo, List<KeyValuePair<SpellKey, float>> inputs)
{
    if (inputs.Count < combo.Count) return false;

    for (int i = inputs.Count - combo.Count, j = 0; i < inputs.Count; i++, j++)
    {
        if (inputs[i].Key != combo[j])
            return false;
    }

    return true;
}

private bool IsPartialComboMatch(List<SpellKey> combo, List<KeyValuePair<SpellKey, float>> inputs)
{
    if (inputs.Count == 0 || combo.Count == 0 || inputs.Count > combo.Count) return false;

    for (int i = 0; i < inputs.Count; i++)
    {
        if (inputs[i].Key != combo[i])
            return false; // Mismatch found, not a partial combo
    }

    return true; // No mismatches found, it's a valid partial combo
}

}

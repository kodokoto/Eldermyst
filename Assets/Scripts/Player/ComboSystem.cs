using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComboSystem : MonoBehaviour
{
    private static List<KeyCode> ValidComboKeys = new List<KeyCode>(
        new KeyCode[] {
            KeyCode.UpArrow,
            KeyCode.DownArrow,
            KeyCode.LeftArrow,
            KeyCode.RightArrow,
            KeyCode.E,
            KeyCode.Q,
        }
    );

    private Queue<KeyValuePair<KeyCode, float>> inputQueue = new Queue<KeyValuePair<KeyCode, float>>();
    private float maxComboDelay = 1f;

    // Define combos
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Check if any key is pressed and the input string is not empty
        if (Input.anyKeyDown)
        {
            Debug.Log("Key pressed");
            EnqueueInput();
            CheckCombos();
            Debug.Log("Current combo: " + string.Join("", inputQueue.Select(kvp => kvp.Key.ToString()).ToArray()));
        }

        ExpireOldInputs();
    }

    private KeyCode GetKeyFromInput()
    {
        foreach (KeyCode key in ValidComboKeys)
        {
            if (Input.GetKeyDown(key))
            {
                return key;
            }
        }
        return KeyCode.None;
    }

    // private List<KeyCode> ParseInputString()
    // {
    //     List<KeyCode> parsedKeys = new List<KeyCode>();
    //     Debug.Log("Parsing input string: " + Input.inputString);
    
    //     foreach (char key in Input.inputString)
    //     {
    //         try
    //         {
    //             KeyCode keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), key.ToString(), true);
    //             Debug.Log("Found Keycode: " + keyCode);
    //             // if keycode is within valid combo keys
    //             if (ValidComboKeys.Contains(keyCode))
    //             {
    //                 parsedKeys.Add(keyCode);
    //             }
    //         }
    //         catch
    //         {
    //             // Ignore invalid key inputs
    //         }
    //     }
    //     return parsedKeys;
    // }

    private void EnqueueInput()
    {
        // List<KeyCode> keyCode = ParseInputString();

        // foreach (KeyCode key in keyCode)
        // {
        //     inputQueue.Enqueue(new KeyValuePair<KeyCode, float>(key, Time.time));
        // }

        KeyCode key = GetKeyFromInput();
        if (key != KeyCode.None)
        {
            inputQueue.Enqueue(new KeyValuePair<KeyCode, float>(key, Time.time));
        }
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
                if (IsComboMatch(sh.Spell.combo))
                {
                    sh.Cast();
                    inputQueue.Clear();
                    break; // Consider if you want to break here or allow overlapping combos
                }
            }
        }
    }

    private bool IsComboMatch(List<KeyCode> combo)
    {
        if (inputQueue.Count < combo.Count) return false;

        var comboQueue = new Queue<KeyCode>(inputQueue.Select(kvp => kvp.Key));
        foreach (var key in combo)
        {
            if (comboQueue.Count == 0 || key != comboQueue.Dequeue())
                return false;
        }

        return true;
    }
}

using UnityEngine;

public class SpellWheelUI : MonoBehaviour
{
    [SerializeField] private Loadout loadout;
    [SerializeField] private GameObject spellButtonPrefab;
    [SerializeField] private float radius;
    private SpellButton[] spellButtons;
    [SerializeField] private KeyCode spellPickerKey;
    private int selectedSpell;
    void Awake()
    {
        // instantiate new buttons for each spell in loadout
        for (int i = 0; i < loadout.Spells.Count; i++)
        {
            // assert that the spell button prefab is not null
            Debug.Assert(spellButtonPrefab != null, "Spell button prefab is null");

            // calculate the angle for the button
            float angle = i * Mathf.PI * 2f / loadout.Spells.Count;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            GameObject spellButton = Instantiate(spellButtonPrefab, transform.position + position, Quaternion.identity, transform);

            // add the spell to the button
            spellButton.GetComponent<SpellButton>().SpellHandler = loadout.SpellHandlers[i];

            // add the button to the array of buttons
            spellButtons[i] = spellButton.GetComponent<SpellButton>();

            // set the button state to disabled
            spellButtons[i].State = SpellButton.ButtonState.Disabled;
        }

        transform.localScale = Vector3.zero;
    }

    void Update()
    {

        if (Input.GetKeyDown(spellPickerKey))
        {
            // show the spell wheel
            transform.localScale = new Vector3(1, 1, 1);

            selectedSpell = GetSelectedSpell();
        }

        if (Input.GetKeyUp(spellPickerKey))
        {
            // hide the spell wheel
            transform.localScale = Vector3.zero;

            // get the currently selected spell
            loadout.SetSpellSlot1(loadout.SpellHandlers[selectedSpell]);
        }

    }

    public int GetSelectedSpell()
    {
        Vector2 normalizedMousePosition = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        float angle = Mathf.Atan2(normalizedMousePosition.y - 0.5f, normalizedMousePosition.x - 0.5f) * Mathf.Rad2Deg;

        angle = (angle + 360) % 360;

        int selected = (int) (angle / 360f * spellButtons.Length);

        if (spellButtons[selected].State != SpellButton.ButtonState.Disabled)
        {
            return selected;
        }

        return selectedSpell;
    }
}
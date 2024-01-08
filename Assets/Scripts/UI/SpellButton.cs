
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
    public enum ButtonState
    {
        Disabled,
        Enabled,
        Hover,
        Selected,
        UsedElsewhere,
    }

    [HideInInspector] public SpellHandler SpellHandler;
    private ButtonState _state;

    public ButtonState State
    {
        get => _state;
        set
        {
            _state = value;
            switch (_state)
            {
                case ButtonState.Disabled:
                    GetComponent<Button>().interactable = false;
                    GetComponent<Image>().color = Color.gray;
                    break;
                case ButtonState.Enabled:
                    GetComponent<Button>().interactable = true;
                    GetComponent<Image>().color = Color.white;
                    break;
                case ButtonState.Hover:
                    GetComponent<Button>().interactable = true;
                    break;
                case ButtonState.Selected:
                    GetComponent<Button>().interactable = true;
                    break;
                case ButtonState.UsedElsewhere:
                    GetComponent<Button>().interactable = false;
                    break;
            }
        }
    }

    public void SetState(ButtonState state)
    {
        State = state;
    }

    void Awake()
    {
        // add the spell name to the text in the middle of the button (textmeshpro)
        GetComponentInChildren<Text>().text = SpellHandler.Spell.name;
        // add the spell icon to the button
        // GetComponentInChildren<Image>().sprite = SpellHandler.Spell.icon;

        // set the button state to disabled
        State = ButtonState.Disabled;
    }

    void Update()
    {
        // if the spell is unlocked, set the button state to enabled
    }


}


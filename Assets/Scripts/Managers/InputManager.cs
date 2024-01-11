using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


[CreateAssetMenu(fileName = "InputManager", menuName = "Managers/InputManager")]
public class InputManager : ScriptableObject, 
                            PlayerInput.IGameplayActions, 
                            PlayerInput.IDialogueActions, 
                            PlayerInput.IPauseMenuActions
{
    public PlayerInput playerInput;
    public event UnityAction MenuSelectEvent;

    // Ingame events
    public event UnityAction PauseEvent = delegate { };
    public event UnityAction<float> MoveEvent;
    public event UnityAction JumpStartedEvent;
    public event UnityAction JumpHeldEvent;
    public event UnityAction JumpCancelEvent;
    public event UnityAction DashEvent;
    public event UnityAction<SpellKey> SpellEvent = delegate { };
    public event UnityAction OpenSpellBookEvent;
    public event UnityAction CloseSpellBookExitEvent;

    // Dialogue events
    public event UnityAction AdvanceEvent;

    // Pause Menu event
    public event UnityAction UnpauseEvent;

    // Spell Book event


    public void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Gameplay.SetCallbacks(this);
            playerInput.Dialogue.SetCallbacks(this);
            playerInput.PauseMenu.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    public void OnDisable()
    {
        DisableAllInput();
    }

    // Gameplay Actions

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>().x);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (JumpStartedEvent != null && context.started)
            JumpStartedEvent.Invoke();
        if (JumpHeldEvent != null && context.performed)
            JumpHeldEvent.Invoke();
        if (JumpCancelEvent != null && context.canceled)
            JumpCancelEvent.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (DashEvent != null && context.started)
            DashEvent.Invoke();
    }

    public void OnComboLeft(InputAction.CallbackContext context)
    {
        if (SpellEvent != null && context.started)
            SpellEvent.Invoke(SpellKey.Left);
    }

    public void OnComboRight(InputAction.CallbackContext context)
    {
        if (SpellEvent != null && context.started)
            SpellEvent.Invoke(SpellKey.Right);
    }

    public void OnComboUp(InputAction.CallbackContext context)
    {
        if (SpellEvent != null && context.started)
            SpellEvent.Invoke(SpellKey.Up);
    }

    public void OnComboDown(InputAction.CallbackContext context)
    {
        if (SpellEvent != null && context.started)
            SpellEvent.Invoke(SpellKey.Down);
    }

    public void OnComboAttack(InputAction.CallbackContext context)
    {
        if (SpellEvent != null && context.started)
            SpellEvent.Invoke(SpellKey.Attack);
    }

    public void OnComboDefend(InputAction.CallbackContext context)
    {  
        if (SpellEvent != null && context.started)
            SpellEvent.Invoke(SpellKey.Defend);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (PauseEvent != null && context.started) {
            PauseEvent.Invoke();
            EnablePauseMenuInput();
        }
    }

    public void OnOpenSpellBook(InputAction.CallbackContext context)
    {
        if (OpenSpellBookEvent != null && context.started)
            OpenSpellBookEvent.Invoke();
        if (CloseSpellBookExitEvent != null && context.canceled)
            CloseSpellBookExitEvent.Invoke();
    }

    // Pause Menu Actions

    public void OnPauseExit(InputAction.CallbackContext context)
    {
        if (UnpauseEvent != null && context.started) {
            UnpauseEvent.Invoke();
            EnableGameplayInput();
        }
    }

    // Dialogue Actions

    public void OnAdvance(InputAction.CallbackContext context)
    {
        if (AdvanceEvent != null && context.started)
            AdvanceEvent.Invoke();
    }

    // Global Actions

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (MenuSelectEvent != null && context.started)
            MenuSelectEvent.Invoke();
    }

    public void EnableGameplayInput()
    {
        Debug.Log("Enable Gameplay Input");
        playerInput.Gameplay.Enable();
        playerInput.Dialogue.Disable();
        playerInput.PauseMenu.Disable();
    }

    public void EnableDialogueInput()
    {
        Debug.Log("Enable Dialogue Input");
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Enable();
        playerInput.PauseMenu.Disable();
    }

    public void EnablePauseMenuInput()
    {
        Debug.Log("Enable Pause Menu Input");
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Disable();
        playerInput.PauseMenu.Enable();
    }

    public void EnableSpellBookInput()
    {
        Debug.Log("Enable Spell Book Input");
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Disable();
        playerInput.PauseMenu.Disable();
    }

    public void DisableAllInput()
    {
        Debug.Log("Disable All Input");
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Disable();
        playerInput.PauseMenu.Disable();
    }
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum SpellKey
{
    Left,
    Right,
    Up,
    Down,
    Attack,
    Defend
}

public class InputManager : ScriptableObject, PlayerInput.IGameplayActions, PlayerInput.IDialogueActions, PlayerInput.IMenuActions
{
    public PlayerInput playerInput;
    public event UnityAction PauseEvent = delegate { };
    public event UnityAction PauseExitEvent;
    public event UnityAction OpenSpellBookEvent;
    public event UnityAction CloseSpellBookExitEvent;

    // Ingame events
    public event UnityAction<float> MoveEvent;
    public event UnityAction JumpStartedEvent;
    public event UnityAction JumpHeldEvent;
    public event UnityAction JumpCancelEvent;
    public event UnityAction DashEvent;
    public event UnityAction<SpellKey> SpellEvent;

    // Dialogue events
    public event UnityAction AdvanceEvent;

    // Menu events
    public event UnityAction MenuExitEvent;
    public event UnityAction MenuSelectEvent;

    public void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Gameplay.SetCallbacks(this);
            playerInput.Dialogue.SetCallbacks(this);
            playerInput.Menu.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    public void OnDisable()
    {
        DisableAllInput();
    }

    public void EnableGameplayInput()
    {
        playerInput.Gameplay.Enable();
        playerInput.Dialogue.Disable();
        playerInput.Menu.Disable();
    }

    public void EnableDialogueInput()
    {
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Enable();
        playerInput.Menu.Disable();
    }

    public void EnableMenuInput()
    {
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Disable();
        playerInput.Menu.Enable();
    }

    public void DisableAllInput()
    {
        playerInput.Gameplay.Disable();
        playerInput.Dialogue.Disable();
        playerInput.Menu.Disable();
    }

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
            EnableMenuInput();
        }
    }

    public void OnSpellBook(InputAction.CallbackContext context)
    {
        if (OpenSpellBookEvent != null && context.started)
            OpenSpellBookEvent.Invoke();
        if (CloseSpellBookExitEvent != null && context.canceled)
            CloseSpellBookExitEvent.Invoke();
    }

    public void OnAdvance(InputAction.CallbackContext context)
    {
        if (AdvanceEvent != null && context.started)
            AdvanceEvent.Invoke();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (MenuSelectEvent != null && context.started)
            MenuSelectEvent.Invoke();
    }

    public void OnMenuExit(InputAction.CallbackContext context)
    {
        if (MenuExitEvent != null && context.started)
            MenuExitEvent.Invoke();
    }
}
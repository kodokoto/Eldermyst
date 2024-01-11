using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _spellBookScreen;
    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private Dialogue _dialogueScreen;

    [Header("Listeners")]
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private DialogueSignalSO _dialogueSignal;
    [SerializeField] private SignalSO _onPlayerDeath;

    [Header("Broadcasts")]
    [SerializeField] private SignalSO _onRetry;
    [SerializeField] private SignalSO _onExitToMenu;
    public void OnEnable()
    {
        _inputManager.PauseEvent += OnPause;
        _inputManager.UnpauseEvent += OnPauseExit;
        _inputManager.OpenSpellBookEvent += OnOpenSpellBook;
        _inputManager.CloseSpellBookExitEvent += OnCloseSpellBookExit;
        _inputManager.AdvanceEvent += OnDialogueAdvanced;

        _dialogueSignal.OnTriggered += OnDialogueEventRaised;
        _onPlayerDeath.OnTriggered += OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        Time.timeScale = 0;
        _deathScreen.SetActive(true);
        Debug.Log("Player died");
    }

    public void OnRetry()
    {
        Debug.Log("Retry");
        Time.timeScale = 1;
        _inputManager.EnableGameplayInput();
        _onRetry.Trigger();
    }

    public void OnDisable()
    {
        _inputManager.PauseEvent -= OnPause;
        _inputManager.UnpauseEvent -= OnPauseExit;
        _inputManager.OpenSpellBookEvent -= OnOpenSpellBook;
        _inputManager.CloseSpellBookExitEvent -= OnCloseSpellBookExit;
        _inputManager.AdvanceEvent -= OnDialogueAdvanced;

        _dialogueSignal.OnTriggered -= OnDialogueEventRaised;
        _onPlayerDeath.OnTriggered -= OnPlayerDeath;
    }

    public void OnExitToMenu()
    {
        Debug.Log("Exit to menu");
        Time.timeScale = 1;
        _inputManager.EnablePauseMenuInput();
        _onExitToMenu.Trigger();
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        _pauseScreen.SetActive(true);
    }

    public void OnPauseExit()
    {
        Time.timeScale = 1;
        _pauseScreen.SetActive(false);
    }

    public void OnOpenSpellBook()
    {
        Time.timeScale = 0;
        _spellBookScreen.SetActive(true);
    }

    public void OnCloseSpellBookExit()
    {
        Time.timeScale = 1;
        _spellBookScreen.SetActive(false);
    }

    public void OnDialogueEventRaised(List<string> dialogue)
    {
        _inputManager.EnableDialogueInput();
        Time.timeScale = 0;
        _dialogueScreen.gameObject.SetActive(true);
        _dialogueScreen.SetDialogue(dialogue);
    }

    private void OnDialogueAdvanced()
    {
        // show next line, if there isnt one, close the dialogue
        if (!_dialogueScreen.Advance())
        {
            Time.timeScale = 1;
            _dialogueScreen.gameObject.SetActive(false);
            _inputManager.EnableGameplayInput();
        }
    }
}
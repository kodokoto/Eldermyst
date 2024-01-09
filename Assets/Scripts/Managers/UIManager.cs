using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// union type for screens

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject spellBookScreen;
    [SerializeField] private Dialogue dialogueScreen;

    [Header("Listeners")]
    [SerializeField] private InputManager _inputManager = default;
    [SerializeField] private DialogueDataChannelSO _dialogueChannel = default;

    public void OnEnable()
    {
        _inputManager.PauseEvent += OnPause;
        _inputManager.UnpauseEvent += OnPauseExit;
        _inputManager.OpenSpellBookEvent += OnOpenSpellBook;
        _inputManager.CloseSpellBookExitEvent += OnCloseSpellBookExit;
        _inputManager.AdvanceEvent += OnDialogueAdvanced;
        _dialogueChannel.OnEventRaised += OnDialogueEventRaised;
    }

    public void OnDisable()
    {
        _inputManager.PauseEvent -= OnPause;
        _inputManager.UnpauseEvent -= OnPauseExit;
        _inputManager.OpenSpellBookEvent -= OnOpenSpellBook;
        _inputManager.CloseSpellBookExitEvent -= OnCloseSpellBookExit;
        _inputManager.AdvanceEvent -= OnDialogueAdvanced;
        _dialogueChannel.OnEventRaised -= OnDialogueEventRaised;
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void OnPauseExit()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }

    public void OnOpenSpellBook()
    {
        Time.timeScale = 0;
        spellBookScreen.SetActive(true);
    }

    public void OnCloseSpellBookExit()
    {
        Time.timeScale = 1;
        spellBookScreen.SetActive(false);
    }

    public void OnDialogueEventRaised(List<string> dialogue)
    {
        _inputManager.EnableDialogueInput();
        Time.timeScale = 0;
        dialogueScreen.gameObject.SetActive(true);
        dialogueScreen.SetDialogue(dialogue);
    }

    private void OnDialogueAdvanced()
    {
        // show next line, if there isnt one, close the dialogue
        if (!dialogueScreen.Advance())
        {
            Time.timeScale = 1;
            dialogueScreen.gameObject.SetActive(false);
            _inputManager.EnableGameplayInput();
        }
    }
}
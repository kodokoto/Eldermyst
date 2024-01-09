using UnityEngine;
using UnityEngine.UI;
// union type for screens

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject spellBookScreen;

    [SerializeField] private InputManager _inputManager = default;


    public void OnEnable()
    {
        _inputManager.PauseEvent += OnPause;
        _inputManager.UnpauseEvent += OnPauseExit;
        _inputManager.OpenSpellBookEvent += OnOpenSpellBook;
        _inputManager.CloseSpellBookExitEvent += OnCloseSpellBookExit;
    }

    public void OnDisable()
    {
        _inputManager.PauseEvent -= OnPause;
        _inputManager.UnpauseEvent -= OnPauseExit;
        _inputManager.OpenSpellBookEvent -= OnOpenSpellBook;
        _inputManager.CloseSpellBookExitEvent -= OnCloseSpellBookExit;
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

    
    // public static UIManager instance;
    
    // private void Awake()
    // {
    //     if (instance == null)
    //         instance = this;
    // }

    // public void ShowGameOverScreen()
    // {
    //     gameObject.transform.Find("GameOverScreen").gameObject.SetActive(true);
    // }

    // public void HideGameOverScreen()
    // {
    //     gameObject.transform.Find("GameOverScreen").gameObject.SetActive(false);
    // }

    // public void ShowWinScreen()
    // {
    //     gameObject.transform.Find("WinScreen").gameObject.SetActive(true);
    // }

    // public void HideWinScreen()
    // {
    //     gameObject.transform.Find("WinScreen").gameObject.SetActive(false);
    // }
}
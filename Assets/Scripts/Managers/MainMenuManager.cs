using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SceneSO _startingScene;
    [SerializeField] private InputManager _inputManager = default;
    [SerializeField] private LoadSceneChannelSO _loadSceneChannel = default;

    // buttons
    [SerializeField] public Button StartNewGameButton;
    [SerializeField] public Button QuitGameButton;

    public UnityAction NewGameButtonAction;


    public void StartNewGame()
    {
        NewGameButtonAction?.Invoke();
        _inputManager.EnableGameplayInput();
        _loadSceneChannel.RaiseEvent(_startingScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
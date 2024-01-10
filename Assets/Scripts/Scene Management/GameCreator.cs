using System.Collections;
using UnityEngine;

public class GameCreator : MonoBehaviour
{
    // [SerializeField] private SaveSystem _saveSystem = default;

    [SerializeField] private SceneSO _startingScene;
    [SerializeField] private LoadSceneChannelSO _loadLevelSignal;
    // [SerializeField] private SimpleEventChannelSO _startNewGameSignal;
    // [SerializeField] private SimpleEventChannelSO _continueGameSignal;



    private bool _hasSavedGame;

    public void Start()
    {
        // load the latest save file
        // _hasSavedGame = _saveSystem.HasSavedGame();
        // _startNewGameSignal.OnTrigger += StartNewGame;
        // _continueGameSignal.OnTrigger += ContinueGame;
    }

    public void StartNewGame()
    {
        _loadLevelSignal.RaiseEvent(_startingScene);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGame());
    }

    public IEnumerator LoadGame()
    {
        yield return null;
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private PlayerSpawnPoint currentSpawnPoint;
    [SerializeField] private PlayerData playerData;
	[SerializeField] private SceneSO _menuToLoad = default;

    [Header("Managers")]
    [SerializeField] private InputManager _inputManager = default;
    [SerializeField] private SaveManager _saveManager = default;

    [Header("Listeners")]
    [SerializeField] private SignalSO _onRetry;
    [SerializeField] private SignalSO _onExit;
    [SerializeField] private SignalSO _onReady;
    [SerializeField] private SpawnPointSignal _spawnPointSignal;

    [Header("Broadcasts")]
    [SerializeField] private LoadSceneSignalSO _loadLevelSignal;
    [SerializeField] private SignalSO _onRestartScene;

    private void OnEnable()
    {
        _spawnPointSignal.OnTriggered += SetSpawnPoint;
        _onRetry.OnTriggered += RestartGame;
        _onExit.OnTriggered += SaveAndQuit;
        _onReady.OnTriggered += OnSceneReady;
    }

    private void OnDisable()
    {
        _spawnPointSignal.OnTriggered -= SetSpawnPoint;
        _onRetry.OnTriggered -= RestartGame;
        _onExit.OnTriggered -= SaveAndQuit;
        _onReady.OnTriggered -= OnSceneReady;
    }

    private void OnSceneReady()
    {
        Debug.Log("Gameplay Scene is ready");
        _inputManager.EnableGameplayInput();

    }

    private void RestartGame()
    {
        playerData.SoftReset();
        _onRestartScene.Trigger();
    }

    private void SaveAndQuit()
    {
        _saveManager.saveFilename = playerData.playerName + ".json";
        _saveManager.SaveGame();
        _loadLevelSignal.Trigger(_menuToLoad);
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        currentSpawnPoint.SetSpawnPoint(spawnPoint);
    }
}
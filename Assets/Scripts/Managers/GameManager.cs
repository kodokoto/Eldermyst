using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public enum GameState {
    Playing,
    Won,
    Lost
}

// Everything that needs to be saved should be in this class
public class GameManager : MonoBehaviour
{
    private GameState gameState;

    [Header("Data")]
    [SerializeField] private PlayerSpawnPoint currentSpawnPoint;
    [SerializeField] private PlayerData playerData;
	[SerializeField] private SceneSO _menuToLoad = default;

    [Header("Managers")]
    [SerializeField] private InputManager _inputManager = default;
    [SerializeField] private SaveManager _saveManager = default;

    [Header("Listeners")]
    [SerializeField] private SimpleEventChannelSO _onRetry;
    [SerializeField] private SimpleEventChannelSO _onExit;
    [SerializeField] private SimpleEventChannelSO _onReady;


    [Header("Broadcasts")]
    [SerializeField] private SpawnPointChangedSignal spawnPointChangedSignal;
    [SerializeField] private LoadSceneChannelSO _loadLevelSignal;
    [SerializeField] private SimpleEventChannelSO _onRestartScene;

    private void OnEnable()
    {
        spawnPointChangedSignal.OnSpawnPointChanged += SetSpawnPoint;
        _onRetry.OnTrigger += RestartGame;
        _onExit.OnTrigger += SaveAndQuit;
        _onReady.OnTrigger += OnSceneReady;
    }

    private void OnSceneReady()
    {
        _inputManager.EnableGameplayInput();
    }

    private void RestartGame()
    {
        playerData.SoftReset();
        _onRestartScene.RaiseEvent();
    }

    private void OnDisable()
    {
        spawnPointChangedSignal.OnSpawnPointChanged -= SetSpawnPoint;
        _onRetry.OnTrigger -= RestartGame;
        _onExit.OnTrigger -= SaveAndQuit;
        _onReady.OnTrigger -= OnSceneReady;
    }

    private void SaveAndQuit()
    {
        _loadLevelSignal.RaiseEvent(_menuToLoad);
    }

    public void OnSave()
    {
        _saveManager.saveFilename = playerData.playerName + ".json";
        _saveManager.SaveGame();
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        currentSpawnPoint.SetSpawnPoint(spawnPoint);
    }
}
// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;
//     public GameState gameState;
//     private void Awake()
//     {
//         if (instance == null) {
//             instance = this;
//         } else {
//             Destroy(gameObject);
//         }
//     }

//     private void Start()
//     {
//         gameState = GameState.Playing;
//     }

//     public void SetGameState(GameState state)
//     {
//         switch (state)
//         {
//             case GameState.Playing:
//                 HandleGamePlaying();
//                 break;
//             case GameState.Won:
//                 HandleGameWon();
//                 break;
//             case GameState.Lost:
//                 HandleGameLost();
//                 break;
//         }
//     }

//     public void Retry()
//     {
//         SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
//     }

//     public void RestartGame()
//     {
//         Destroy(gameObject);
//         instance = null;
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     public void MainMenu()
//     {
//         // reset spawn point
//         PlayerSpawnPoint.instance.SetSpawnPoint(Vector3.zero);
//         SceneManager.LoadSceneAsync("MainMenu");
//     }

//     public GameState GetGameState()
//     {
//         return gameState;
//     }

//     // handlers

//     private void HandleGameLost()
//     {
//         // Debug.Log("Game lost");
//         // if (gameState == GameState.Playing)
//         // {
//         //     Debug.Log("Game lost for real");
//         //     gameState = GameState.Lost;
//         //     UIManager.instance.ShowGameOverScreen();
//         // }
//         // else
//         // {
//         //     Debug.LogWarning("Tried to set game state to lost when it is not playing");
//         // }
//     }

//     private void HandleGameWon()
//     {
//         // if (gameState == GameState.Playing)
//         // {
//         //     gameState = GameState.Won;
//         //     UIManager.instance.ShowWinScreen();
//         // }
//         // else
//         // {
//         //     Debug.LogWarning("Tried to set game state to won when it is not playing");
//         // }
//     }

//     private void HandleGamePlaying()
//     {
//         // if (gameState == GameState.Won)
//         // {
//         //     gameState = GameState.Playing;
//         //     UIManager.instance.HideWinScreen();
//         // }
//         // else if (gameState == GameState.Lost)
//         // {
//         //     gameState = GameState.Playing;
//         //     UIManager.instance.HideGameOverScreen();
//         // }
//         // else
//         // {
//         //     Debug.LogWarning("Tried to set game state to playing when it is already playing");
//         // }
//     }

// }

using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // [SerializeField] private SceneSO _gameplayScene;

    [SerializeField] private LoadSceneChannelSO _loadMenuChannel;
    [SerializeField] private LoadSceneChannelSO _loadLevelChannel;
    [SerializeField] private SimpleEventChannelSO _onSceneReady;
    // private SceneSO _gameplaySceneInstance;
    private SceneSO _currentScene;
    private SceneSO _sceneToLoad;

    public void OnEnable()
    {
        _loadMenuChannel.OnLoadingRequested += LoadMenu;
        _loadLevelChannel.OnLoadingRequested += LoadLevel;
    }

    public void OnDisable()
    {
        _loadMenuChannel.OnLoadingRequested -= LoadMenu;
        _loadLevelChannel.OnLoadingRequested -= LoadLevel;
    }

    private void LoadMenu(SceneSO menu)
    {
        _sceneToLoad = menu;
        // unload the persistent gameplay manager scene if it's loaded
        // if (_gameplaySceneInstance != null && _gameplaySceneInstance.Scene.isLoaded)
        // {
        //     SceneManager.UnloadSceneAsync(_gameplaySceneInstance.Scene);
        // }

        UnloadScene();
    }

    private void UnloadScene()
    {
        // if its null, we're coming from initialization, so we don't need to unload anything
        if (_currentScene != null)
        {
            _currentScene.sceneReference.UnLoadScene();
        }
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0).Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        _currentScene = _sceneToLoad;

		Scene s = handle.Result.Scene;
		SceneManager.SetActiveScene(s);
        StartGameplay();
    }

    private void LoadLevel(SceneSO level)
    {
        _sceneToLoad = level;
        
        // // if the gameplay scene is not loaded, load it
        // if (_gameplaySceneInstance == null || !_gameplaySceneInstance.Scene.isLoaded)
        // {
        //     SceneManager.LoadSceneAsync(_gameplayScene.name, LoadSceneMode.Additive).completed += OnGameplaySceneLoaded;
        // }
        // else
        // {
        //     UnloadScene();
        // }
        UnloadScene();

    }

    private void OnGameplaySceneLoaded(AsyncOperation obj)
    {
        // _gameplaySceneInstance = _gameplayScene;
        UnloadScene();
    }

    private void StartGameplay()
	{
		_onSceneReady.RaiseEvent(); //Spawn system will spawn the PigChef in a gameplay scene
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}
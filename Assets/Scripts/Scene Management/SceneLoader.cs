using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // [SerializeField] private SceneSO _gameplayScene;

    [Header("Listeners")]
    [SerializeField] private LoadSceneChannelSO _loadMenuChannel;
    [SerializeField] private LoadSceneChannelSO _loadLevelChannel;
    [SerializeField] private SimpleEventChannelSO _onSceneReload;


    [Header("Broadcasts")]
    [SerializeField] private SimpleEventChannelSO _onSceneReady;

    // private SceneSO _gameplaySceneInstance;
    private SceneSO _currentScene;
    private SceneSO _sceneToLoad;

    public void OnEnable()
    {
        _loadMenuChannel.OnLoadingRequested += LoadMenu;
        _loadLevelChannel.OnLoadingRequested += LoadLevel;
        _onSceneReload.OnTrigger += ReloadScene;
    }

    public void Update()
    {
        Debug.Log(SceneManager.sceneCount);
    }

    public void OnDisable()
    {
        _loadMenuChannel.OnLoadingRequested -= LoadMenu;
        _loadLevelChannel.OnLoadingRequested -= LoadLevel;
        _onSceneReload.OnTrigger -= ReloadScene;
    }

    private void ReloadScene()
    {
        _sceneToLoad = _currentScene;
        UnloadScene();
        // 
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
        Debug.Log("Unloading scene");
        // if its null, we're coming from initialization, so we don't need to unload anything
        if (_currentScene != null)
        {
			if (_currentScene.sceneReference.OperationHandle.IsValid())
			{
                Debug.Log("Unloading scene through addressables");
                Addressables.UnloadSceneAsync(_currentScene.sceneReference.OperationHandle);
			}
            // webgl sucks
            else if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                SceneManager.UnloadSceneAsync(_currentScene.sceneName);
            }
        #if UNITY_EDITOR
			else
			{
				SceneManager.UnloadSceneAsync(_currentScene.sceneReference.editorAsset.name);
			}
        #endif        
        }
        LoadNewScene(default);
    }

    private void LoadNewScene(AsyncOperationHandle<SceneInstance> handle)
    {
        Debug.Log("Old scene unloaded, loading new scene");
        Addressables.LoadSceneAsync(_sceneToLoad.sceneReference, LoadSceneMode.Additive, true, 0).Completed += OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
    {
        Debug.Log("New scene loaded");
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
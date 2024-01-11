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
                //Unload the scene through its AsyncOperationHandle
				// /_currentScene.sceneReference.UnLoadScene();
                //Unload the scene through its AssetReference, i.e. through the Addressable system
                Addressables.UnloadSceneAsync(_currentScene.sceneReference.OperationHandle);
			}
        #if UNITY_EDITOR
			else
			{
                Debug.Log("LOLL");
				//Only used when, after a "cold start", the player moves to a new scene
				//Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
				//the scene needs to be unloaded using regular SceneManager instead of as an Addressable
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
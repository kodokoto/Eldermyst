using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Listeners")]
    [SerializeField] private LoadSceneSignalSO _loadMenuSignal;
    [SerializeField] private LoadSceneSignalSO _loadLevelSignal;
    [SerializeField] private SignalSO _onSceneReload;


    [Header("Broadcasts")]
    [SerializeField] private SignalSO _onSceneReady;

    private SceneSO _currentScene;
    private SceneSO _sceneToLoad;

    public void OnEnable()
    {
        _loadMenuSignal.OnTriggered += LoadMenu;
        _loadLevelSignal.OnTriggered += LoadLevel;
        _onSceneReload.OnTriggered += ReloadScene;
    }

    public void OnDisable()
    {
        _loadMenuSignal.OnTriggered -= LoadMenu;
        _loadLevelSignal.OnTriggered -= LoadLevel;
        _onSceneReload.OnTriggered -= ReloadScene;
    }

    private void ReloadScene()
    {
        _sceneToLoad = _currentScene;
        UnloadScene();
    }

    private void LoadMenu(SceneSO menu)
    {
        _sceneToLoad = menu;
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
        UnloadScene();
    }

    private void StartGameplay()
	{
		_onSceneReady.Trigger(); //Spawn system will spawn the PigChef in a gameplay scene
	}
}
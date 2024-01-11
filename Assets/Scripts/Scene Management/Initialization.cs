using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Initialization : MonoBehaviour
{
	[SerializeField] private SceneSO _managersScene = default;
	[SerializeField] private SceneSO _menuToLoad = default;
    [SerializeField] private AssetReference _loadMenuSignal = default;

	private void Start()
	{
		//Load the persistent managers scene
		_managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnManagersSceneLoaded;
	}

	private void OnManagersSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		//Load the menu scene asset
		_loadMenuSignal.LoadAssetAsync<LoadSceneSignalSO>().Completed += LoadMainMenu;
	}

    private void LoadMainMenu(AsyncOperationHandle<LoadSceneSignalSO> handle)
    {
		// Broadcast the load menu signal to load the menu scene
        handle.Result.Trigger(_menuToLoad);
		SceneManager.UnloadSceneAsync(0); // unload the initialization scene
    }
}

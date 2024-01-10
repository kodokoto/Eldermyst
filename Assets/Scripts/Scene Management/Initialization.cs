using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class Initialization : MonoBehaviour
{
	[SerializeField] private SceneSO _managersScene = default;
	[SerializeField] private SceneSO _menuToLoad = default;
    [SerializeField] private AssetReference _loadMenuChannel = default;

	private void Start()
	{
		//Load the persistent managers scene
		_managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += OnManagersSceneLoaded;
	}

	private void OnManagersSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		_loadMenuChannel.LoadAssetAsync<LoadSceneChannelSO>().Completed += LoadMainMenu;
	}

    private void LoadMainMenu(AsyncOperationHandle<LoadSceneChannelSO> handle)
    {
        handle.Result.RaiseEvent(_menuToLoad);
		SceneManager.UnloadSceneAsync(0); // unload the initialization scene
    }
}

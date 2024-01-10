

using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SaveSystem : ScriptableObject
{
    [SerializeField] private PlayerData _playerData = default;
    [SerializeField] private PlayerInventory _playerInventory = default;
    [SerializeField] private PlayerSpawnPoint _playerSpawnPoint = default;
    // load scene signal
    [SerializeField] private LoadSceneChannelSO _loadSceneSignal = default;

    private string saveFilename = "save.json";

    private GameSave _gameSave = new GameSave();
    
    public void Start()
    {
        _loadSceneSignal.OnLoadingRequested += SetCurrentScene;
    }

    public void SetCurrentScene(SceneSO scene)
    {
        if (scene.sceneType == SceneType.Level)
        {
            _gameSave._scene_guid = scene.Guid;
        }

        SaveGame();
    }

    public void SaveGame()
    {
        _gameSave._spell_guids.Clear();
        foreach (Spell spell in _playerInventory.Spells)
        {
            _gameSave._spell_guids.Add(spell.Guid);
        }

        _gameSave._player_data_guid = _playerData.Guid;

        _gameSave._player_spawn_point_guid = _playerSpawnPoint.Guid;
        if (FileManager.WriteToFile(saveFilename, _gameSave.ToJson()))
        {
            //Debug.Log("Save successful " + saveFilename);
        }
    }

    public IEnumerator LoadInventory()
    {
        _playerInventory.Spells.Clear();
        foreach (string spellGuid in _gameSave._spell_guids)
        {
            var loadSpellOperationHandle = Addressables.LoadAssetAsync<Spell>(spellGuid);
			yield return loadSpellOperationHandle;
			if (loadSpellOperationHandle.Status == AsyncOperationStatus.Succeeded)
			{
				var spell = loadSpellOperationHandle.Result;
				_playerInventory.AddSpell(spell);
			}
        }
    }

    public IEnumerator LoadPlayerData()
    {
        var loadPlayerDataOperationHandle = Addressables.LoadAssetAsync<PlayerData>(_gameSave._player_data_guid);
        yield return loadPlayerDataOperationHandle;
        if (loadPlayerDataOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var playerData = loadPlayerDataOperationHandle.Result;
            _playerData.SetPlayerData(playerData);
        }
    }

    public bool HasSavedGame()
    {
        //if there is a save file, 
        //  load the most recent save file
        //  return true    
        // else
        return false;
    }
}
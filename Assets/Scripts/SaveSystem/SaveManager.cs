

using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(menuName = "Managers/Save Manager")]
public class SaveManager : ScriptableObject
{
    [Header("Data")]
    [SerializeField] private PlayerData _playerData = default;
    [SerializeField] private PlayerInventory _playerInventory = default;
    [SerializeField] private PlayerSpawnPoint _playerSpawnPoint = default;

    [Header("Broadcasts")]
    [SerializeField] private LoadSceneSignalSO _loadSceneSignal = default;

    public string saveFilename = "save.json";

    public GameSave _gameSave = new GameSave();
    
    public void OnEnable()
    {
        _loadSceneSignal.OnTriggered += SetCurrentScene;
    }

    public void OnDisable()
    {
        _loadSceneSignal.OnTriggered -= SetCurrentScene;
    }

    public void SetCurrentScene(SceneSO scene)
    {
        if (scene.sceneType == SceneType.Level)
        {
            Debug.Log("Setting current scene to " + scene.name);
            Debug.Assert(scene.Guid != null, "scene.Guid is null");
            Debug.Log(scene.name +" = " + scene.Guid);
            _gameSave._scene_guid = scene.Guid;
        }
    }

    public void CreateNewSave(string saveName)
    {
        saveFilename = saveName;
        _playerData.HardReset();
        _playerInventory.Reset();
        _playerSpawnPoint.Reset();
        _playerData.playerName = saveName;
    }

    public void SaveGame()
    {
        _gameSave._spell_guids.Clear();
        foreach (Spell spell in _playerInventory.Spells)
        {
            _gameSave._spell_guids.Add(spell.Guid);
            Debug.Log("Added spell " + spell.name + " to save object");
        }        

        _gameSave._player_data_guid = _playerData.Guid;
        Debug.Assert(_playerData.Guid != null, "_playerData.Guid is null");
        Debug.Log("Added player data " + _playerData.playerName + " to save object");
        Debug.Log("Player data guid " + _playerData.Guid);

        _gameSave._player_spawn_point_guid = _playerSpawnPoint.Guid;
        Debug.Log("Added player spawn point " + _playerSpawnPoint.name + " to save object");
        if (FileManager.WriteToFile(saveFilename, _gameSave.ToJson()))
        {
            Debug.Log("Save successful " + saveFilename);
        }
    }

    public bool LoadSaveDataFromDisk()
	{
		if (FileManager.LoadFromFile(saveFilename, out var json))
		{
			_gameSave.LoadFromJson(json);
			return true;
		}

		return false;
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
        Debug.Log("Loaded " + _playerInventory.Spells.Count + " spells");
    }

    public IEnumerator LoadPlayerData()
    {
        Debug.Log("Loading player data " + _gameSave._player_data_guid);
        var loadPlayerDataOperationHandle = Addressables.LoadAssetAsync<PlayerData>(_gameSave._player_data_guid);
        yield return loadPlayerDataOperationHandle;
        if (loadPlayerDataOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var playerData = loadPlayerDataOperationHandle.Result;
            _playerData.SetPlayerData(playerData);
        }
        Debug.Log("Loaded player data " + _playerData.playerName);
    }

    public IEnumerator LoadPlayerSpawnPoint()
    {
        Debug.Log("Loading player spawn point " + _gameSave._player_spawn_point_guid);
        var loadPlayerSpawnPointOperationHandle = Addressables.LoadAssetAsync<PlayerSpawnPoint>(_gameSave._player_spawn_point_guid);
        yield return loadPlayerSpawnPointOperationHandle;
        if (loadPlayerSpawnPointOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            var playerSpawnPoint = loadPlayerSpawnPointOperationHandle.Result;
            _playerSpawnPoint.SetSpawnPoint(playerSpawnPoint.GetSpawnPoint());
        }
        Debug.Log("Loaded player spawn point " + _playerSpawnPoint.name);
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
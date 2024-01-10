using System.Collections.Generic;
using UnityEngine;

public class GameSave
{
    public string _scene_guid;

    public List<string> _spell_guids;

    public string _player_data_guid;

    public string _player_spawn_point_guid;

    public string ToJson()
	{
		return JsonUtility.ToJson(this);
	}

	public void LoadFromJson(string json)
	{
		JsonUtility.FromJsonOverwrite(json, this);
	}
}
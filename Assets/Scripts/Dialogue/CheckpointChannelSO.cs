
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Spawn Point Changed Channel")]
public class SpawnPointChannelSO : ScriptableObject
{
	public UnityAction<Vector3> OnSpawnPointChanged;
	
	public void RaiseEvent(Vector3 SpawnPoint)
	{
		OnSpawnPointChanged?.Invoke(SpawnPoint);
	}
}

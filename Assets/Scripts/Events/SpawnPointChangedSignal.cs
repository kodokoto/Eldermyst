
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Spawn Point Changed Signal")]
public class SpawnPointChangedSignal : ScriptableObject
{
	public UnityAction<Vector3> OnSpawnPointChanged;
	
	public void RaiseEvent(Vector3 SpawnPoint)
	{
		Debug.Log("Spawn Point Event Raised");
		OnSpawnPointChanged?.Invoke(SpawnPoint);
	}
}

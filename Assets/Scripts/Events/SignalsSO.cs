using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalSO : ScriptableObject
{
	public UnityAction OnTriggered;
	
	public void Trigger()
	{
		OnTriggered?.Invoke();
	}
}

public class SignalSO<T> : ScriptableObject
{
	public UnityAction<T> OnTriggered;
	
	public void Trigger(T obj)
	{
		OnTriggered?.Invoke(obj);
	}
}

[CreateAssetMenu(menuName = "Signals/Dialogue Signal")]
public class DialogueSignalSO : SignalSO<List<string>> {}

[CreateAssetMenu(menuName = "Signals/Spawn Point Changed Signal")]
public class SpawnPointChangedSignal : SignalSO<Vector3> {}

[CreateAssetMenu(menuName = "Signals/Load Scene Channel")]
public class LoadSceneSignalSO : SignalSO<SceneSO> {}



using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Signals/Signal")]
public class SignalSO : SerializableScriptableObject
{
	public UnityAction OnTriggered;
	
	public void Trigger()
	{
		OnTriggered?.Invoke();
	}
}

public class SignalSO<T> : SerializableScriptableObject
{
	public UnityAction<T> OnTriggered;
	
	public void Trigger(T obj)
	{
		OnTriggered?.Invoke(obj);
	}
}





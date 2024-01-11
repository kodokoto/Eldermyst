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





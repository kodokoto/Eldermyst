using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used for scene-loading events.
/// Takes a GameSceneSO of the location or menu that needs to be loaded, and a bool to specify if a loading screen needs to display.
/// </summary>
[CreateAssetMenu(menuName = "Events/Simple Event Channel")]
public class SimpleEventChannelSO : ScriptableObject
{
	public UnityAction OnTrigger;

	public void RaiseEvent()
	{
		OnTrigger?.Invoke();
	}
}


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Dialogue Data Channel")]
public class DialogueDataChannelSO : ScriptableObject
{
	public UnityAction<List<string>> OnDialogueTriggered;
	
	public void RaiseEvent(List<string> dialogue)
	{
		OnDialogueTriggered?.Invoke(dialogue);
	}
}

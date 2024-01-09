using System.Collections.Generic;
using UnityEngine;

public class DialogueEventTrigger : MonoBehaviour
{
    [SerializeField] private List<string> dialogue;
    [SerializeField] private DialogueDataChannelSO dialogueChannel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueChannel.RaiseEvent(dialogue);
        }
    }
}
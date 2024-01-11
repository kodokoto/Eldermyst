using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] private List<string> dialogue;
    [SerializeField] private DialogueSignalSO dialogueSignal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueSignal.Trigger(dialogue);
        }
    }
}
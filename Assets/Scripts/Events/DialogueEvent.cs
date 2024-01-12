using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    [SerializeField] private List<string> dialogue;
    [SerializeField] private DialogueSignalSO dialogueSignal;
    [SerializeField] private bool _destructabe = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dialogueSignal.Trigger(dialogue);
            if (_destructabe)
            {
                Destroy(gameObject);
            }
        }
    }
}
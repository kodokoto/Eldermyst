using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpawnPointChangedSignal spawnPointChangedSignal;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint");
            spawnPointChangedSignal.RaiseEvent(transform.position);
            gameObject.SetActive(false);
        }
    }
}

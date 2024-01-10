using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chckpoint : MonoBehaviour
{
    [SerializeField] private SpawnPointChannelSO spawnPointChannel;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint");
            spawnPointChannel.RaiseEvent(transform.position);
            gameObject.SetActive(false);
        }
    }
}

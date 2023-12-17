using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint");
            PlayerSpawnPoint.instance.spawnPoint = transform.position;
            Debug.Log("Player spawn point set to " + PlayerSpawnPoint.instance.spawnPoint);
            gameObject.SetActive(false);
            // GameManager.instance.SetGameState(GameState.Won);
        }
    }
}

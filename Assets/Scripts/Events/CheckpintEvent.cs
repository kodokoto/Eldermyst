using UnityEngine;

public class CheckpointEvent : MonoBehaviour
{
    [SerializeField] private SpawnPointChangedSignal spawnPointChangedSignal;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered checkpoint");
            spawnPointChangedSignal.Trigger(transform.position);
            gameObject.SetActive(false);
        }
    }
}

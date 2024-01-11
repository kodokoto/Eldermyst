using UnityEngine;

public class CheckpointEvent : MonoBehaviour
{
    [SerializeField] private SpawnPointSignal spawnPointChangedSignal;

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

using UnityEngine;

public class CheckpointEvent : MonoBehaviour
{
    [SerializeField] private SpawnPointSignal spawnPointChangedSignal;
    [SerializeField] private AudioSignalSO checkpointAudioSignal;
    [SerializeField] private AudioClip checkpointSFX;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            checkpointAudioSignal.Trigger(checkpointSFX, transform.position, 20f);
            Debug.Log("Player entered checkpoint");
            spawnPointChangedSignal.Trigger(transform.position);
            gameObject.SetActive(false);
        }
    }
}

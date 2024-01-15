using UnityEngine;

public class CheckpointEvent : MonoBehaviour
{
    [SerializeField] private SpawnPointSignal spawnPointChangedSignal;
    [SerializeField] private AudioSignalSO checkpointAudioSignal;
    [SerializeField] private AudioClip checkpointSFX;
    private bool reached = false;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !reached)
        {
            checkpointAudioSignal.Trigger(checkpointSFX, transform.position, 20f);
            Debug.Log("Player entered checkpoint");
            spawnPointChangedSignal.Trigger(transform.position);
            // Get Light component
            Light light = GetComponentInChildren<Light>();
            light.intensity = 1f;
            reached = true;
        }
    }
}

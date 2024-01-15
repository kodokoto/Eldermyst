using UnityEngine;

public abstract class Pickup : MonoBehaviour
{

    [SerializeField] protected AudioSignalSO _pickupAudioSignal = default;
    [SerializeField] protected AudioClip _pickupSFX = default;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPickup(collision.gameObject);
        }
    }

    protected abstract void OnPickup(GameObject actor);
}
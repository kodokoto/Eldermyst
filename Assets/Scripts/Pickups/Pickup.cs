using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPickup(collision.gameObject);
        }
    }

    protected abstract void OnPickup(GameObject actor);
}
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // GameManager.instance.SetGameState(GameState.Won);
        }
    }
}


using UnityEngine;


public class NextLevelEvent : MonoBehaviour
{
    [SerializeField] private SceneSO _nextLevel;
    [SerializeField] private LoadSceneSignalSO _nextLevelSignal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _nextLevelSignal.Trigger(_nextLevel);
        }
    }
}
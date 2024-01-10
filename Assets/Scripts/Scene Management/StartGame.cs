using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField] private SceneSO _startingScene;
    [SerializeField] private LoadSceneChannelSO _loadGameplayChannel;

    public void StartNewGame()
    {
        _loadGameplayChannel.RaiseEvent(_startingScene);
    }
}
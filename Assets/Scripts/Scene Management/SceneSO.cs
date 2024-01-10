using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;


public enum SceneType
{
    Level,
    Menu,
    Manager
}

[CreateAssetMenu(menuName = "Scene Management/SceneSO")]
public class SceneSO : SerializableScriptableObject
{
    public AssetReference sceneReference;
    public SceneType sceneType;
    // audio
}
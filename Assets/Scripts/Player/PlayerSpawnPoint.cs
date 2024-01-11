using UnityEngine;

[CreateAssetMenu(menuName = "GameData/PlayerSpawnPoint")]
public class PlayerSpawnPoint : SerializableScriptableObject
{
    [SerializeField] private Vector3 spawnPoint;
    private Vector3 currentSpawnPoint;

    public void Awake()
    {
        Debug.Log("Spawn Point Awake");
        currentSpawnPoint = spawnPoint;
    } 
    public Vector3 GetSpawnPoint()
    {
        Debug.Log("Spawn Point Get");
        return currentSpawnPoint;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        Debug.Log("Spawn Point Changed to " + newSpawnPoint);
        currentSpawnPoint = newSpawnPoint;
    }

    public void Reset()
    {
        Debug.Log("Spawn Point Reset");
        currentSpawnPoint = spawnPoint;
    }
}
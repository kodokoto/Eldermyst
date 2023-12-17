using UnityEngine;

class PlayerSpawnPoint : MonoBehaviour
{
    [HideInInspector] public Vector3 spawnPoint;
    public static PlayerSpawnPoint instance;
    private void Awake()
    {
        Debug.Log("PlayerSpawnPoint Awake");
        if (instance == null)
        {
            Debug.Log("PlayerSpawnPoint instance is null");
            instance = this;
        }
        else
        {
            Debug.Log("PlayerSpawnPoint instance is not null");
            Destroy(gameObject); // destroy the new one
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetSpawnPoint(Vector3 point)
    {
        spawnPoint = point;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint;
    }
}
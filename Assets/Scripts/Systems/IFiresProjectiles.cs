using UnityEngine;

public interface IFiresProjectiles
{
    [field: SerializeField] Transform ProjectileSpawnPoint { get; set; }
}
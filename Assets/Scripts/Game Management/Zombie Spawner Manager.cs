using UnityEngine;

public class ZombieSpawnerManager : MonoBehaviour
{
    // TODO: Integrated Zombie Spawner
    // [SerializeField] private ZombieSpawner[] _zombieSpawner; 
    public void SetupSpawners(int zombieCount)
    {
        
    }
    public void SpawnZombie(ZombieModifier zombieModifier)
    {
        // TODO: Find valid spawners and spawn.
        // var validSpawners = _zombieSpawner.Where(x => x.AvailableSpawns > 0).ToArray;
        // if (validSpawners.Length <= 0) return;
        // var randomSpawner = validSpawners[Random.Range(0, validSpawners.Length)];
        // randomSpawner.Spawn(zombieModifier);
    }
}

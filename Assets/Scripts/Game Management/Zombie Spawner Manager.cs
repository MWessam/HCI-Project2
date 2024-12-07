using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ZombieSpawnerManager : MonoBehaviour
{
    // TODO: Integrated Zombie Spawner
    [SerializeField] private SpawnerControroller[] _zombieSpawners; 
    public void SetupSpawners(int zombieCount)
    {
        var zombieCountPerSpawner = zombieCount / _zombieSpawners.Length;
        var remainingZombies = zombieCount % _zombieSpawners.Length;
        foreach (var zombieSpawner in _zombieSpawners)
        {
            zombieSpawner.ResetSpawner();
            zombieSpawner.maxZombies = zombieCountPerSpawner;
        }
        _zombieSpawners[Random.Range(0, _zombieSpawners.Length)].maxZombies += remainingZombies;
    }
    public void SpawnZombie(ZombieModifier zombieModifier)
    {
        // TODO: Find valid spawners and spawn.
        var validSpawners = _zombieSpawners.Where(x => x.zombiesSpawned < x.maxZombies).ToArray();
        if (validSpawners.Length <= 0) return;
        var randomSpawner = validSpawners[Random.Range(0, validSpawners.Length)];
        randomSpawner.Spawn(zombieModifier);
    }
}

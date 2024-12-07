using System;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class SpawnerControroller : MonoBehaviour
{
    public ZombieController zombiePrefab; 
    public int maxZombies = 5;  
    public Vector2 spawnCooldownRange = new Vector2(6, 10); 
    public int zombiesSpawned = 0;
    private IObjectPool<ZombieController> _zombiePool;

    private void Awake()
    {
        _zombiePool = new ObjectPool<ZombieController>(
            () => Instantiate(zombiePrefab),
            (x) => x.gameObject.SetActive(true),
            (x) => x.gameObject.SetActive(false),
            (x) => Destroy(x));
    }

    // public void SpawnZombies()
    // {
    //     if (zombiesSpawned < maxZombies)
    //     {
    //         float cooldown = Random.Range(spawnCooldownRange.x, spawnCooldownRange.y);
    //         Invoke(nameof(Spawn), cooldown);
    //     }
    // }

    public void ResetSpawner()
    {
        zombiesSpawned = 0;
        maxZombies = 0;
    }

    public void Spawn(ZombieModifier zombieModifier)
    {
        zombiesSpawned++;
        var zombie = _zombiePool.Get();
        zombie.transform.position = transform.position;
        zombie.transform.rotation = Quaternion.identity;
        zombie.currentHealth = zombie.maxHealth;
        zombie.Spawn(zombieModifier);
        zombie.OnZombieDead += () =>
        {
            _zombiePool.Release(zombie);
            WaveManager2.Instance.OnZombieDead(zombie);
            zombie.OnZombieDead = null;
        };
    }
}

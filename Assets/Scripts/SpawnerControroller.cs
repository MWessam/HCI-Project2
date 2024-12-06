using UnityEngine;

public class SpawnerControroller
{
    public GameObject zombiePrefab; 
    public int maxZombies = 5;  
    public Vector2 spawnCooldownRange = new Vector2(6, 10); 
    private int zombiesSpawned = 0;

    public void SpawnZombies()
    {
        if (zombiesSpawned < maxZombies)
        {
            float cooldown = Random.Range(spawnCooldownRange.x, spawnCooldownRange.y);
            Invoke(nameof(Spawn), cooldown);
        }
    }

    private void Spawn()
    {
        Instantiate(zombiePrefab, transform.position, Quaternion.identity);
        zombiesSpawned++;
        SpawnZombies();
    }
}

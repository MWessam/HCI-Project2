using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveData> _waveData;
    public event Action<ZombieModifier> OnZombieShouldSpawn;
    public event Action<int> OnWaveStart; 
    public event Action OnWaveEnd;
    private int _currentZombieCount;
    private int _zombiesSpawned;
    public void StartWave(int waveNumber)
    {
        if (waveNumber <= 0)
        {
            waveNumber = 0;
        }
        else if (waveNumber >= _waveData.Count)
        {
            waveNumber = _waveData.Count - 1;
        }
        var wave = _waveData[waveNumber];
        ResetWaveState();
        OnWaveStart?.Invoke(wave.WaveZombieCount);
        StartWaveSpawning(wave);
    }

    private void StartWaveSpawning(WaveData waveData)
    {
        StartCoroutine(WaveLoop(waveData));
    }

    private void ResetWaveState()
    {
        _currentZombieCount = 0;
        _zombiesSpawned = 0;
    }

    private IEnumerator WaveLoop(WaveData waveData)
    {
        while (_zombiesSpawned < waveData.WaveZombieCount)
        {
            if (_currentZombieCount <= waveData.MaxZombieCountAtOnce)
            {
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(waveData.ZombieSpawnTimingRange.x,
                waveData.ZombieSpawnTimingRange.y));
            OnZombieShouldSpawn?.Invoke(waveData.ZombieModifier);
        }
        OnWaveEnd?.Invoke();
    }
}

[Serializable]
public class WaveData
{
    public int WaveNumber;
    public int WaveZombieCount;
    public int MaxZombieCountAtOnce;
    public Vector2 ZombieSpawnTimingRange;
    public ZombieModifier ZombieModifier;
}

[Serializable]
public class ZombieModifier
{
    [Range(1, 4)]
    public float SpeedModifier;
    public float HealthModifier;
}

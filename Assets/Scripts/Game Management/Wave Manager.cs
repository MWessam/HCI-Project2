using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager2 : MonoBehaviour
{
    public static WaveManager2 Instance;
    [SerializeField] private List<WaveData> _waveData;
    public event Action<ZombieModifier> OnZombieShouldSpawn;
    public event Action<int> OnWaveStart; 
    public event Action OnWaveEnd;
    private int _currentZombieCount;
    private int _zombiesSpawned;
    [SerializeField] private TMP_Text _waveText;

    private void Awake()
    {
        Instance = this;
    }

    public void StartWave(int waveNumber)
    {
        _waveText.text = $"Wave: {waveNumber}";
        float waveCountModifier = 1;
        if (waveNumber <= 0)
        {
            waveNumber = 1;
        }
        else if (waveNumber >= _waveData.Count)
        {
            waveCountModifier = (waveNumber - _waveData.Count) * 1.2f;
            waveNumber = _waveData.Count - 1;
        }
        var wave = _waveData[waveNumber - 1];
        ResetWaveState();
        OnWaveStart?.Invoke((int)(wave.WaveZombieCount * waveCountModifier));
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
            if (_currentZombieCount >= waveData.MaxZombieCountAtOnce)
            {
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(waveData.ZombieSpawnTimingRange.x,
                waveData.ZombieSpawnTimingRange.y));
            _currentZombieCount++;
            _zombiesSpawned++;
            OnZombieShouldSpawn?.Invoke(waveData.ZombieModifier);
        }

        while (_currentZombieCount > 0)
        {
            yield return null;
        }
        OnWaveEnd?.Invoke();
    }

    public void OnZombieDead(ZombieController zombie)
    {
        --_currentZombieCount;
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

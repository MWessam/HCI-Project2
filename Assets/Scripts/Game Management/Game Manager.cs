using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WaveManager2 _waveManager;
    [SerializeField] private ZombieSpawnerManager _zombieSpawnerManager;
    [SerializeField] private GameObject _gameOverUi;
    private int _currentWave = 1;
    private void OnEnable()
    {
        _waveManager.OnWaveStart += _zombieSpawnerManager.SetupSpawners;
        _waveManager.OnZombieShouldSpawn += _zombieSpawnerManager.SpawnZombie;
        _waveManager.OnWaveEnd += OnWaveEnd;
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;
        _gameOverUi.gameObject.SetActive(true);
    }

    private void OnWaveEnd()
    {
        _currentWave++;
        StartCoroutine(WaveCooldown());
    }

    private IEnumerator WaveCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _waveManager.StartWave(_currentWave);
    }

    void Start()
    {
        _currentWave = 1;
        _waveManager.StartWave(_currentWave);
        Player.Instance.OnPlayerDeath += OnGameOver;
    }

    void Update()
    {
        
    }
}

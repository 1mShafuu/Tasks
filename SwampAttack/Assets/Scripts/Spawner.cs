using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Player _player;

    private Wave _currentWave;
    private int _currentWaveNumber;
    private float _timeAfterLastSpawn;
    private int _spawned;

    public event UnityAction AllEnemySpawned;
    public event UnityAction<int, int> EnemyCountChanged;

    private void Awake()
    {
        _currentWave = new Wave();
    }

    private void Start()
    {
        SetWave(_currentWaveNumber);   
    }

    private void Update()
    {
        if (_currentWave == null)
        {
            return;
        }

        _timeAfterLastSpawn += Time.deltaTime;
        
        if (_timeAfterLastSpawn >= _currentWave.Delay)
        {
            InstantiateEnemy();
            _spawned++;
            _timeAfterLastSpawn = 0;
            EnemyCountChanged?.Invoke(_spawned, _currentWave.Count);
        }

        if (_currentWave.Count <= _spawned)
        {
            if (_waves.Count > _currentWaveNumber + 1)
            {
                AllEnemySpawned?.Invoke();
            }
            
            _currentWave = null;
        }
    }

    public void NextWave()
    {
        SetWave(++_currentWaveNumber);
        _spawned = 0;
    }
    
    private void InstantiateEnemy()
    {
        var randomNumber = Random.Range(0, _currentWave.Templates.Count);
        Enemy enemy = Instantiate(_currentWave.Templates[randomNumber], _spawnPoint.position, _spawnPoint.rotation, _spawnPoint)
            .GetComponent<Enemy>();
        enemy.Init(_player);
        enemy.Dying += OnEnemyDied;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Dying -= OnEnemyDied;
        _player.AddMoney(enemy.Reward);
    }
    
    private void SetWave(int index)
    {
        _currentWave = _waves[index];
        EnemyCountChanged?.Invoke(0,1);
    }
}

[Serializable]
public class Wave
{
    public float Delay;
    public int Count;
    public List<GameObject> Templates = new List<GameObject>();
}

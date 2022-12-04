using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : ObjectPool
{
    [SerializeField] private List<GameObject> _obstacleTemplates;
    [SerializeField] private GameObject _levelTemplate;
    [SerializeField] private float _secondsBetweenSpawn;
    [SerializeField] private float _minSpawnPositionZ;
    [SerializeField] private float _maxSpawnPositionZ;
    [SerializeField] private Player _player;
    [SerializeField] private SpeedBooster _speedBooster;
    
    private float _elapsedTime = 0;
    private int _activeTilesOnStart = 4;
    private List<GameObject> _activeTiles = new List<GameObject>();
    private float _spawnOffset = 0;
    private float _tileLenght = 100f;
    private float _startSecondsBetweenSpawn;
    private int _tileOffset = 60;
    private Vector3 _generatorSpawnPosition;
    private Vector3 _playerSpawnPosition;
    
    private void Awake()
    {
        Initialize(_obstacleTemplates, _levelTemplate);
        _startSecondsBetweenSpawn = _secondsBetweenSpawn;
        _generatorSpawnPosition = transform.position;
        _playerSpawnPosition = _player.StartPosition;
        SpawnAllStartTiles();
    }

    private void Start()
    {
        transform.position = _playerSpawnPosition + _generatorSpawnPosition;
    }

    private void Update()
    {
        //Debug.Log(_generatorSpawnPosition);
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime > _secondsBetweenSpawn)
        {
            if (TryGetObject(out GameObject obstacle))
            {
                _elapsedTime = 0;
                float spawnPosition = Random.Range(_minSpawnPositionZ, _maxSpawnPositionZ);
                Vector3 spawnPoint = transform.position;
                spawnPoint.z = spawnPosition;
                spawnPoint.y = _generatorSpawnPosition.y;
                obstacle.SetActive(true);
                obstacle.transform.position = spawnPoint;
                DisableObjectAbroadScreen();
            }
        }
        
        if (_player.transform.position.x - _tileOffset > _spawnOffset - (_activeTilesOnStart * _tileLenght))
        {
            SpawnTile();
            DeleteTile();
        }     
    }

    private void OnEnable()
    {
        _speedBooster.SpeedChanged += OnSpeedChange;
    }

    private void OnDisable()
    {
        _speedBooster.SpeedChanged -= OnSpeedChange;
    }

    public void Restart()
    {
        for (int index = 0; index < _activeTilesOnStart; index++)
        {
            DeleteTile();
        }
        
        Debug.Log("DELETED");
        _spawnOffset = 0;
        SpawnAllStartTiles();
    }
    
    private void SpawnTile()
    {
        GameObject nextTile = Instantiate(_levelTemplate, _player.transform.right * _spawnOffset, _player.transform.rotation);
        _activeTiles.Add(nextTile);
        _spawnOffset += _tileLenght;
    }

    private void DeleteTile()
    {
        Destroy(_activeTiles[0]);
        _activeTiles.RemoveAt(0);
    }

    private void SpawnAllStartTiles()
    {
        for (int index = 0; index < _activeTilesOnStart; index++)
        {
            SpawnTile();
        }
    }
    
    private void OnSpeedChange(float speed)
    {
        var startSpeed = _speedBooster.StartSpeed;
        Debug.Log($"{startSpeed} , {speed},  {_startSecondsBetweenSpawn}");
        _secondsBetweenSpawn = (_startSecondsBetweenSpawn + 0.05f) / (speed / startSpeed);
    }
}

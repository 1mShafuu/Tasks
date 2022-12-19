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
    [SerializeField] private int _lineDistance;
    [SerializeField] private Player _player;

    private SpeedMagnifier _speedMagnifier;
    private float _elapsedTime = 0;
    private int _activeTilesOnStart = 4;
    private List<GameObject> _activeTiles;
    private float _spawnOffset = 0;
    private float _tileLenght = 100f;
    private float _startSecondsBetweenSpawn;
    private int _tileOffset = 60;
    private Vector3 _generatorSpawnPosition;
    private Vector3 _playerSpawnPosition;
    private int[] _possibleLines;
    
    private void Awake()
    {
        _possibleLines = new int[] {(int)_playerSpawnPosition.z,-_lineDistance,_lineDistance};
        _activeTiles = new List<GameObject>();
        _speedMagnifier = _player.GetComponent<SpeedMagnifier>();
        Initialize(_obstacleTemplates);
        _startSecondsBetweenSpawn = _secondsBetweenSpawn;
        _generatorSpawnPosition = transform.position;
        _playerSpawnPosition = _player.StartPosition;
        SpawnAllStartTiles();
    }
    
    private void OnEnable()
    {
        _speedMagnifier.SpeedChanged += OnSpeedChange;
    }

    private void OnDisable()
    {
        _speedMagnifier.SpeedChanged -= OnSpeedChange;
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
                int spawnPosition = _possibleLines[Random.Range(0,_possibleLines.Length)];
                Vector3 spawnPoint = transform.position;
                spawnPoint.z = spawnPosition;
                //Debug.Log(_generatorSpawnPosition.y);
                spawnPoint.y = _generatorSpawnPosition.y;
               // Debug.Log(spawnPoint);
                obstacle.SetActive(true);
                obstacle.transform.position = spawnPoint;
                //Debug.Log(transform.TransformPoint(obstacle.transform.position));
                DisableObjectAbroadScreen();
            }
        }
        
        if (_player.transform.position.x - _tileOffset > _spawnOffset - (_activeTilesOnStart * _tileLenght))
        {
            SpawnTile();
            DeleteTile();
        }     
    }

    public void RestartObjects()
    {
        for (int index = 0; index < _activeTilesOnStart; index++)
        {
            DeleteTile();
        }
        
        Initialize(_obstacleTemplates);
        _spawnOffset = 0;
        _secondsBetweenSpawn = _startSecondsBetweenSpawn;
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
        const float inaccuracy = 0.0043f; 
        var startSpeed = _speedMagnifier.StartSpeed;
        _secondsBetweenSpawn = (_startSecondsBetweenSpawn + inaccuracy * inaccuracy) / (speed / startSpeed);
    }
}

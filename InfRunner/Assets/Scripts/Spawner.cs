using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : ObjectPool
{
    [SerializeField] private List<GameObject> _enemyTemplates;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private float _secondsBetweenSpawn;

    private float _elapasedTime = 0;

    private void Start()
    {
        Initialize(_enemyTemplates);
    }

    private void Update()
    {
        _elapasedTime += Time.deltaTime;

        if (_elapasedTime >= _secondsBetweenSpawn)
        {
            if (TryGetObject(out GameObject enemy))
            {
                _elapasedTime = 0;
                int spawnPointNumber = Random.Range(0, _spawnPoints.Count);
                SetEnemy(enemy, _spawnPoints[spawnPointNumber].position);
            }
        }
    }

    private void SetEnemy(GameObject enemy,Vector3 spawnPoint)
    {
        enemy.SetActive(true);
        enemy.transform.position = spawnPoint;
    }
}

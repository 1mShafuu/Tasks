using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin coin;
    [SerializeField] private Transform points;
    [SerializeField] private int enemyCount;

    private int _count;
    private List<Transform> _spawnPoints;
    private Coroutine _spawn;

    private void Start()
    {
        _spawnPoints = new List<Transform>();

        for (int index = 0; index < points.childCount; index++)
        {
            _spawnPoints.Add(points.GetChild(index));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_spawn != null)
                StopCoroutine(_spawn);
            int currentPoint = GetSpawnpointNumber();
            StartCoroutine(SpawnEnemies(currentPoint));
        }
    }

    private IEnumerator SpawnEnemies(int currentPoint)
    {
        float secondsForWait = 2.0f;
        var waitForSeconds = new WaitForSeconds(secondsForWait);
        var radiusMultiplier = 4f;

        for (int index = 0; index < enemyCount; index++)
        {
            Instantiate(coin, _spawnPoints[currentPoint].position + Random.insideUnitSphere * radiusMultiplier,Quaternion.identity);
            
            yield return waitForSeconds;
        }
    }
    
    private int GetSpawnpointNumber()
    {
        return Random.Range(0, _spawnPoints.Count - 1);
    }
}

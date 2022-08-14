using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform points;
    [SerializeField] private int enemyCount;

    private int _count;
    private List<Transform> _spawnPoints;
    private Coroutine _spawn;

    private void Start()
    {
        Debug.Log(points.childCount);
        _spawnPoints = new List<Transform>();

        for (int index = 0; index < points.childCount; index++)
        {
            _spawnPoints.Add(points.GetChild(index));
            Debug.Log(_spawnPoints[index]);
        }
        Debug.Log( "Spawnpoints"+_spawnPoints.Count);

    }

    private void OnMouseDown()
    {
        if (_spawn != null)
            StopCoroutine(_spawn);
        int currentPoint = GetSpawnpointNumber();
        StartCoroutine(SpawnEnemies(currentPoint));
    }

    private IEnumerator SpawnEnemies(int currentPoint)
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var radiusMultiplier = 4f;

        for (int index = 0; index < enemyCount; index++)
        {
            Instantiate(enemy,  _spawnPoints[currentPoint].position + Random.insideUnitSphere * radiusMultiplier,Quaternion.identity);
            
            yield return waitForSeconds;
        }
    }
    
    private int GetSpawnpointNumber()
    {
        return Random.Range(0, _spawnPoints.Count);
    }
}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _obstacleContainer;
    [SerializeField] private int _obstacleCapacity;

    private Camera _camera;
    private List<GameObject> _obstaclePool = new List<GameObject>();

    public void ResetPool()
    {
        foreach (var prefab in _obstaclePool)
        {
            prefab.SetActive(false);
        }   
    }

    protected void Initialize(List<GameObject> obstaclePrefabs, GameObject levelPrefab)
    {
        _camera = Camera.main;
        
        for (int index = 0; index < _obstacleCapacity; index++)
        {
            var randomPrefabNumber = Random.Range(0, obstaclePrefabs.Count);
            var obstaclePrefab = obstaclePrefabs[randomPrefabNumber];
            GameObject spawmed = Instantiate(obstaclePrefab, _obstacleContainer.transform);
            spawmed.SetActive(false);
            _obstaclePool.Add(spawmed);
        }
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _obstaclePool.FirstOrDefault(p => p.activeSelf == false);
        return result != null;
    }

    protected void DisableObjectAbroadScreen()
    {
        Vector3 disablePoint = _camera.ViewportToWorldPoint(new Vector2(0, 2f));
        
        foreach (var obstacle in _obstaclePool)
        {
            if (obstacle.activeSelf == true)
            {
                if (obstacle.transform.position.x < disablePoint.x)
                {
                    obstacle.SetActive(false);
                }
            }
        }
    }
}

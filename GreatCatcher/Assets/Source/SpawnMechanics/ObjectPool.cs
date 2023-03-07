using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _animalsContainer;
    [SerializeField] protected int SheepCapacity;
    [SerializeField] protected int CowCapacity;
    [SerializeField] protected int BullCapacity;
    [SerializeField] protected int ChickenCapacity;
    
    private readonly List<GameObject> _animalsPool = new List<GameObject>();

    public void ResetPool()
    {
        foreach (var prefab in _animalsPool)
        {
            prefab.SetActive(false);
        }   
    }

    protected void Initialize(GameObject prefab, int spawnAmountPrefabs)
    {
        for (int index = 0; index < spawnAmountPrefabs; index++)
        {
            var currentPrefab = prefab;
            GameObject spawned = Instantiate(currentPrefab, _animalsContainer.transform);
            spawned.SetActive(false);
            _animalsPool.Add(spawned);
        }
    }

    protected bool TryGetObject(out GameObject result)
    {
        result = _animalsPool.FirstOrDefault(obstacle => obstacle.activeSelf == false);
        return result != null;
    }

    protected void ClearPool()
    {
        for (int index = 0; index < _animalsPool.Count; index++)
        {
            Destroy(_animalsPool[index]);
        }
        
        _animalsPool.Clear();
    }

    protected void ShufflePool()
    {
        var firstElement = 0;
        
        for (int index = 0; index < _animalsPool.Count; index++)
        {
            GameObject temp = _animalsPool[firstElement];
            _animalsPool.RemoveAt(firstElement);
            _animalsPool.Insert(Random.Range(firstElement, _animalsPool.Count + 1), temp);
        }
    }

    protected GameObject GetChicken()
    {
        return _animalsPool.FirstOrDefault(obstacle => obstacle.activeSelf == false && obstacle.TryGetComponent(out Chicken chicken));
    }
}

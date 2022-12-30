using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _animalsContainer;
    [SerializeField] private int _animalsCapacity;
    
    private readonly List<GameObject> _animalsPool = new List<GameObject>();

    public void ResetPool()
    {
        foreach (var prefab in _animalsPool)
        {
            prefab.SetActive(false);
        }   
    }

    protected void Initialize(List<GameObject> prefabs)
    {
        for (int index = 0; index < _animalsPool.Count; index++)
        {
            Destroy(_animalsPool[index]);
        }
        
        _animalsPool.Clear();

        for (int index = 0; index < _animalsCapacity; index++)
        {
            var randomPrefabNumber = Random.Range(0, prefabs.Count);
            var currentPrefab = prefabs[randomPrefabNumber];
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
}

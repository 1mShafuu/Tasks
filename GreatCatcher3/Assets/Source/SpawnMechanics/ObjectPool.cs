using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _animalsContainer;
    [SerializeField] private GameObject _caughtAnimalsContainer;
    [SerializeField] protected int SheepCapacity;
    [SerializeField] protected int CowCapacity;
    [SerializeField] protected int BullCapacity;
    [SerializeField] protected int ChickenCapacity;
    
    private readonly List<GameObject> _animalsPool = new List<GameObject>();
    private readonly List<GameObject> _caughtAnimalsPool = new List<GameObject>();
    private List<GameObject> _availableAnimalsPool = new List<GameObject>();

    public void ResetPool()
    {
        foreach (var prefab in _animalsPool)
        {
            prefab.SetActive(false);
        }   
    }

    protected void Initialize(GameObject prefab, int spawnAmountPrefabs, Player player = null)
    {
        for (int index = 0; index < spawnAmountPrefabs; index++)
        {
            var currentPrefab = prefab;
            
            if (player == null)
            {
                GameObject spawned = Instantiate(currentPrefab, _animalsContainer.transform);
                spawned.SetActive(false);
                _animalsPool.Add(spawned);
            }
            else
            {
                GameObject spawned = Instantiate(currentPrefab, _caughtAnimalsContainer.transform);
                spawned.SetActive(false);
                _caughtAnimalsPool.Add(spawned);
            }
        }
    }

    protected bool TryGetObjectFromAnimalsPool(out GameObject result, bool isCaught = false, GameObject caughtAnimalGameObject = null)
    {
        if (isCaught)
        {
            Animal caughtAnimal = null;
            if (caughtAnimalGameObject != null) caughtAnimalGameObject.TryGetComponent(out caughtAnimal);
            
            result = _caughtAnimalsPool.FirstOrDefault(obstacle => 
                caughtAnimal != null && obstacle.activeSelf == false && obstacle.TryGetComponent(out Animal animal) &&
                animal.GetType() == caughtAnimal.GetType());
        }
        else
        {
            result = _animalsPool.FirstOrDefault(obstacle => obstacle.activeSelf == false);
        }
        
        return result != null;
    }
    
    protected void DeactivateAllCaughtAnimals()
    {
        foreach (var caughtAnimal in _caughtAnimalsPool)
        {
            caughtAnimal.SetActive(false);
        }
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
        var animalsToKeepList = new List<GameObject>();

        var sheepCount = Mathf.Max(Random.Range(10, _animalsPool.Count(a => a.GetComponent<Sheep>() != null)), SheepCapacity);
        animalsToKeepList.AddRange(GetRandomAnimals(sheepCount, typeof(Sheep)));

        var cowCount = Mathf.Max(Random.Range(5, _animalsPool.Count(a => a.GetComponent<Cow>() != null)), CowCapacity);
        animalsToKeepList.AddRange(GetRandomAnimals(cowCount, typeof(Cow)));

        var bullCount = Mathf.Max(Random.Range(2, _animalsPool.Count(a => a.GetComponent<Bull>() != null)), BullCapacity);
        animalsToKeepList.AddRange(GetRandomAnimals(bullCount, typeof(Bull)));

        foreach (var animalToKeep in animalsToKeepList)
        {
            animalToKeep.SetActive(false);
        }

        foreach (var animal in _animalsPool.Where(animal => !animalsToKeepList.Contains(animal)))
        {
            animal.SetActive(false);
        }

        for (int i = 0; i < animalsToKeepList.Count; i++)
        {
            var temp = animalsToKeepList[i];
            var randomIndex = Random.Range(i, animalsToKeepList.Count);
            animalsToKeepList[i] = animalsToKeepList[randomIndex];
            animalsToKeepList[randomIndex] = temp;
        }

        var tempChicken = GetChicken();
        _animalsPool.Clear();
        _animalsPool.AddRange(animalsToKeepList);
        _animalsPool.Add(tempChicken);
    }

    protected GameObject GetChicken()
    {
        return _animalsPool.FirstOrDefault(obstacle => obstacle.activeSelf == false && obstacle.TryGetComponent(out Chicken chicken));
    }
    
    private List<GameObject> GetRandomAnimals(int count, Type animalType)
    {
        var animals = _animalsPool.Where(a => a.GetComponent(animalType) != null && !a.activeSelf).ToList();
        var animalsToKeep = new List<GameObject>();

        while (animalsToKeep.Count < count && animals.Count > 0)
        {
            var randomIndex = Random.Range(0, animals.Count);
            var animal = animals[randomIndex];
            animalsToKeep.Add(animal);
            animals.RemoveAt(randomIndex);
        }

        return animalsToKeep;
    }
    
    protected void ActivateAnimalFromPool(out GameObject activatedAnimal)
    {
        _availableAnimalsPool.Clear();

        foreach (var animal in _animalsPool.Where(animal => !animal.activeSelf && !animal.TryGetComponent(out Chicken chicken)))
        {
            _availableAnimalsPool.Add(animal);
        }

        if (_availableAnimalsPool.Count > 0)
        {
            var randomIndex = Random.Range(0, _availableAnimalsPool.Count);
            activatedAnimal = _availableAnimalsPool[randomIndex];
        }
        else
        {
            activatedAnimal = null;
        }
    }

    protected void DeactivateAnimalInPool(GameObject animal)
    {
        animal.SetActive(false);
    }
}

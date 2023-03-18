using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Agava.YandexGames;

public class SellArea : MonoBehaviour
{
    [SerializeField] private AnimalsUnloader _unloader;
    
    private List<GameObject> _animals = new List<GameObject>();
    private int _currentYardLevel = 1;
    private int _changedYardLevel;
    private int _maxAmountOfAnimals = 3;
    
    public IReadOnlyList<GameObject> Animals => _animals;

    public event Action AnimalsLimitReached;
    public event Action AnimalsLimitNotReached;

    private void OnEnable()
    {
        _unloader.YardChose += OnYardChose;
    }

    private void OnDisable()
    {
        _unloader.YardChose -= OnYardChose;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animal animal))
        {
            if (_animals.Count < _maxAmountOfAnimals)
            {
                _animals.Add(animal.gameObject);
                AnimalsLimitNotReached?.Invoke();
            }
            else
            {
                _animals.Add(animal.gameObject);
                AnimalsLimitReached?.Invoke();
            }
        }
    }
    
    public void ClearDeletedAnimals()
    {
        if (_animals.All(clearedAnimal => clearedAnimal.activeInHierarchy))
        {
            _animals.Clear();
           AnimalsLimitNotReached?.Invoke();
        }
    }

    private void OnYardChose(int value)
    {
        _changedYardLevel = value;

        if (_currentYardLevel != _changedYardLevel)
        {
            _currentYardLevel = value;
            _maxAmountOfAnimals *= 2;
        }
    }
}

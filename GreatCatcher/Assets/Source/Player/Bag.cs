using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private int _maxAmountOfAnimalsInBag = 3;
    private AnimalsUnloader _unloader;
    private CatchArea _catchArea;
    private List<GameObject> _catchedAnimals;
    
    public int AnimalsInBag => _catchedAnimals.Count;
    public IReadOnlyList<GameObject> CatchedAnimals => _catchedAnimals;
    public int MaxAmountOfAnimalsInBag => _maxAmountOfAnimalsInBag;

    private void Awake()
    {
        _catchedAnimals = new List<GameObject>();
        _catchArea = GetComponentInChildren<CatchArea>();
        _unloader = GetComponent<AnimalsUnloader>();
    }

    private void OnEnable()
    {
        _catchArea.AnimalCatched += OnAnimalCatched;
        _unloader.AnimalUnloaded += OnAnimalUnloaded;
    }

    private void OnDisable()
    {
        _catchArea.AnimalCatched -= OnAnimalCatched;
        _unloader.AnimalUnloaded -= OnAnimalUnloaded;
    }
    

    private void OnAnimalCatched(GameObject animal)
    {
        _catchedAnimals.Add(animal);
    }

    private void OnAnimalUnloaded()
    {
        const int firstElementIndex = 0;
        _catchedAnimals.RemoveAt(firstElementIndex);
    }
}

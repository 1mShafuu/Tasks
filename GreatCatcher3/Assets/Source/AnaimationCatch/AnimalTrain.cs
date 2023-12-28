using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class AnimalTrain : ObjectPool
{
    private const int MaxPossibleAnimals = 3;
    
    [SerializeField] private List<GameObject> _animalTemplates;
    [SerializeField] private Player _player;
    [SerializeField] private AnimalsUnloader _animalsUnloader;
    
    private CatchArea _catchArea;

    public event Action<Transform> AnimalAdded;
    public event Action AnimalsDeleted;

    private void OnEnable()
    {
        _catchArea.AnimalCaught += OnAnimalCaught;
        _animalsUnloader.AnimalUnloaded += OnAnimalUnloaded;
    }

    private void OnDisable()
    {
        _catchArea.AnimalCaught -= OnAnimalCaught;
        _animalsUnloader.AnimalUnloaded -= OnAnimalUnloaded;
    }

    private void Awake()
    {
        SheepCapacity = MaxPossibleAnimals;
        CowCapacity = MaxPossibleAnimals;
        BullCapacity = MaxPossibleAnimals;
        ChickenCapacity = MaxPossibleAnimals;
        GameObject sheep = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Sheep>());
        GameObject cow = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Cow>());
        GameObject bull = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Bull>());
        GameObject chicken = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Chicken>());
        _catchArea = _player.GetComponentInChildren<CatchArea>();
        ClearPool();
        Initialize(sheep,SheepCapacity, _player);
        Initialize(cow, CowCapacity, _player);
        Initialize(bull, BullCapacity, _player);
        Initialize(chicken, ChickenCapacity, _player);
    }

    private void OnAnimalCaught(GameObject gameObject)
    {
        ActivateAnimal(gameObject);
    }

    private void OnAnimalUnloaded()
    {
        DeactivateAllCaughtAnimals();
        AnimalsDeleted?.Invoke();
    }
    
    private void ActivateAnimal(GameObject animalGameObject)
    {
        if (!TryGetObjectFromAnimalsPool(out var animal, true, animalGameObject)) return;
        
        const int activationRadius = 5;
        const float spawnPositionY = 3f;
        Vector3 spawnPosition = Random.insideUnitSphere * activationRadius + _player.transform.position;
        spawnPosition.y = spawnPositionY;
        animal.TryGetComponent(out Animal caughtAnimal);
        caughtAnimal.Catch();
        animal.TryGetComponent(out Transform transform);
        animal.SetActive(true);
        animal.transform.position = spawnPosition;
        AnimalAdded?.Invoke(transform);
    }
}

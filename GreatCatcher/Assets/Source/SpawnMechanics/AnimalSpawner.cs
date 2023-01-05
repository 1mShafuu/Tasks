using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : ObjectPool
{
    private const int SpawnRadius = 40;
    private const int AmountAllowedActiveAnimals = 2;
    
    [SerializeField] private List<GameObject> _animalTemplates;
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private Player _player;
    
    private float _elapsedTime = 0;
    private Vector3 _playerSpawnPosition;
    private int _currentAnimalsAmount;
    private CatchArea _catchArea;
    private GameObject _animal;

    private bool CanAddMoreAnimals => _currentAnimalsAmount < AmountAllowedActiveAnimals;
    
    private void Awake()
    {
        _catchArea = _player.GetComponent<CatchArea>();
        Initialize(_animalTemplates);
        _currentAnimalsAmount = 0;
    }
    
    private void OnEnable()
    {
        _catchArea.AnimalCatched += OnAnimalCatched;
    }

    private void OnDisable()
    {
        _catchArea.AnimalCatched -= OnAnimalCatched;
    }

    private void Start()
    {
        for (int index = 0; index < AmountAllowedActiveAnimals; index++)
        {
            ActivateAnimal();
        }
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime >= _spawnCooldown && CanAddMoreAnimals)
        {
            _elapsedTime = 0;
            ActivateAnimal();
        }
    }

    private void ActivateAnimal()
    {
        if (TryGetObject(out _animal))
        {
            const float spawnPositionY = 3f;
            Vector3 spawnPosition = Random.insideUnitSphere * SpawnRadius + transform.position;
            spawnPosition.y = spawnPositionY;
            _animal.SetActive(true);
            _animal.transform.position = spawnPosition;
            _currentAnimalsAmount++;
        }
    }

    private void OnAnimalCatched(GameObject animal)
    {
        if (_currentAnimalsAmount > -1)
        {
            _currentAnimalsAmount--;
        }
    }
}

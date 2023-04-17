using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AnimalSpawner : ObjectPool
{
    private const int SpawnRadius = 70;
    private const int AmountAllowedActiveAnimals = 13;
    
    [SerializeField] private List<GameObject> _animalTemplates;
    [SerializeField] private float _spawnCooldown;
    [SerializeField] private Player _player;
    
    private float _elapsedTime = 0f;
    private float _elapsedTimeForChickenSpawn = 0f;
    private Vector3 _playerSpawnPosition;
    private int _currentAnimalsAmount;
    private CatchArea _catchArea;
    private GameObject _animal;

    private bool CanAddMoreAnimals => _currentAnimalsAmount < AmountAllowedActiveAnimals;

    private void Awake()
    {
        GameObject sheep = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Sheep>());
        GameObject cow = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Cow>());
        GameObject bull = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Bull>());
        GameObject chicken = _animalTemplates.FirstOrDefault(animal => animal.GetComponent<Chicken>());
        _catchArea = _player.GetComponentInChildren<CatchArea>();
        ClearPool();
        Initialize(sheep,SheepCapacity);
        Initialize(cow, CowCapacity);
        Initialize(bull, BullCapacity);
        Initialize(chicken, ChickenCapacity);
        _currentAnimalsAmount = 0;
        ShufflePool();
    }
    
    private void OnEnable()
    {
        _catchArea.AnimalCaught += OnAnimalCaught;
    }

    private void OnDisable()
    {
        _catchArea.AnimalCaught -= OnAnimalCaught;
    }

    private void Start()
    {
        StartCoroutine(SpawnChicken());
        
        for (int index = 0; index < AmountAllowedActiveAnimals; index++)
        {
            ActivateAnimal();
        }
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _elapsedTimeForChickenSpawn += Time.deltaTime;
        
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

    private void OnAnimalCaught(GameObject animal)
    {
        if (_currentAnimalsAmount > 0)
        {
            _currentAnimalsAmount--;
        }
    }
    
    private IEnumerator SpawnChicken()
    {
        const int secondsInOneMinute = 60;
        const int secondsInTwoMinutes = secondsInOneMinute * 2;
        const int secondsBetweenSpawn = secondsInOneMinute * 3;
        int randomSecondsBetweenSpawn = Random.Range(secondsInOneMinute, secondsInTwoMinutes);
        var waitForSecondsForSpawned = new WaitForSeconds(randomSecondsBetweenSpawn);
        var waitForSecondsBetweenSpawns = new WaitForSeconds(secondsBetweenSpawn);
        var foundedChicken = GetChicken();
        
        while (true)
        {
            yield return waitForSecondsBetweenSpawns;

            if (foundedChicken != null)
            {
                if (foundedChicken.activeSelf == false)
                {
                    foundedChicken.SetActive(true);
                    yield return waitForSecondsForSpawned;
                    foundedChicken.SetActive(false);
                }
            }
        }
    }
}

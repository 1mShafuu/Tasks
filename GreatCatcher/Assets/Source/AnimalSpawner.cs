using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalSpawner : ObjectPool
{
    [SerializeField] private List<GameObject> _animalTemplates;
    [SerializeField] private Transform _spawnArea;

    private float _elapsedTime = 0;
    private Vector3 _playerSpawnPosition;
    private int _spawnRadius = 50;

    private void Awake()
    {
        Initialize(_animalTemplates);
    }
    
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (TryGetObject(out GameObject animal))
        {
            const float spawnPositionY = 1f;
            Vector3 spawnPosition = Random.insideUnitSphere * _spawnRadius + transform.position;
            spawnPosition.y = spawnPositionY;
            Debug.Log(spawnPosition);
            animal.SetActive(true);
            animal.transform.position = spawnPosition;
        }
    }
}

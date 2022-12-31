using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private const int MaxPortableObjects = 3;

    [SerializeField] private Player _player;
    
    private CatchArea _catchArea;
    private int _bagAnimalsCount = 0;

    private void Awake()
    {
        _catchArea = _player.GetComponent<CatchArea>();
    }

    private void OnEnable()
    {
        _catchArea.AnimalCatched += OnAnimalCatched;
    }

    private void OnDisable()
    {
        _catchArea.AnimalCatched -= OnAnimalCatched;
    }
    

    private void OnAnimalCatched()
    {
        _bagAnimalsCount++;
    }
}

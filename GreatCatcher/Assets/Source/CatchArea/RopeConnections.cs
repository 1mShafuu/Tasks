using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RopeToolkit;
using RopeToolkit.Example;
using UnityEngine;

public class RopeConnections : MonoBehaviour
{
    [SerializeField] private AnimalTrain _animalTrain;

    private List<DynamicAttach> _dynamicAttachConnections;

    private void Awake()
    {
        _dynamicAttachConnections = new List<DynamicAttach>(GetComponents<DynamicAttach>());
    }

    private void OnEnable()
    {
        _animalTrain.AnimalAdded += OnAnimalAdded;
        _animalTrain.AnimalsDeleted += OnAnimalsDeleted;
    }
    
    private void OnDisable()
    {
        _animalTrain.AnimalAdded -= OnAnimalAdded;
        _animalTrain.AnimalsDeleted -= OnAnimalsDeleted;
    }
    
    private void OnAnimalAdded(Transform gameObjectTransform)
    {
        foreach (var dynamicAttachConnection in _dynamicAttachConnections)
        {
            if (dynamicAttachConnection.target != null) continue;
            
            dynamicAttachConnection.target = gameObjectTransform;
            dynamicAttachConnection.Attach();
            break;
        }
    }

    private void OnAnimalsDeleted()
    {
        foreach (var dynamicAttachConnection in _dynamicAttachConnections)
        {
            dynamicAttachConnection.target = null;
            dynamicAttachConnection.Detach();
        }
    }
}

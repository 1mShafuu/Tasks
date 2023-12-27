using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsThief : MonoBehaviour
{
    public const float MinDistance = 1.5f;
    
    [SerializeField] private Transform _yardPositionForTheft;
    [SerializeField] private Transform _stolenAnimalsPosition;

    private int _targetAmountOfStolenAnimals = 2;
    private SphereCollider _collider;
    private AnimalsThiefMovement _animalsThiefMovement;
    private int _stolenAnimalsForLastMove = 0;

    public Vector3 TargetMovement => _yardPositionForTheft.position;
    
    public event Action AnimalsAlreadyStolen;
    
    private void OnEnable()
    {
        _collider = GetComponent<SphereCollider>();
        _animalsThiefMovement = GetComponent<AnimalsThiefMovement>();
        _animalsThiefMovement.EndPositionReached += OnEndPositionReached;
        _collider.enabled = false;
        _stolenAnimalsForLastMove = 0;
    }

    private void OnDisable()
    {
        _animalsThiefMovement.EndPositionReached -= OnEndPositionReached;
    }

    private void Update()
    {
        var distanceeBetweenTargets = Vector3.Distance(transform.position, _yardPositionForTheft.position);
        var extendedMinDistance = MinDistance * 3;
        
        if (distanceeBetweenTargets <= extendedMinDistance)
        {
            _collider.enabled = true;

            if (_stolenAnimalsForLastMove < _targetAmountOfStolenAnimals &&
                (!(distanceeBetweenTargets < MinDistance))) return;
            
            _collider.enabled = false;
            AnimalsAlreadyStolen?.Invoke();
        }
        else
        {
            _collider.enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Animal animal))
        {
            StealAnimals(animal);
        }
    }

    private void StealAnimals(Animal animal)
    {
        if (_stolenAnimalsForLastMove >= _targetAmountOfStolenAnimals) return;

        animal.gameObject.TryGetComponent(out AnimalMovement movement);
        movement.enabled = false;
        animal.gameObject.transform.position = _stolenAnimalsPosition.position;
        _stolenAnimalsForLastMove++;
    }

    private void OnEndPositionReached()
    {
        _stolenAnimalsForLastMove = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsThief : MonoBehaviour
{
    private const float MinDistance = 1.5f;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Transform _yardPositionForTheft;
    [SerializeField] private Transform _stolenAnimalsPosition;
    
    private int _targetAmountOfStolenAnimals = 2;
    private SphereCollider _collider;
    private int _stolenAnimals = 0;

    public Vector3 TargetMovement => _yardPositionForTheft.position;
    
    public event Action AnimalsAlreadyStolen;

    private void OnEnable()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.enabled = false;
        _stolenAnimals = 0;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _yardPositionForTheft.position) < MinDistance)
        {
            _collider.enabled = true;
            Debug.Log(_stolenAnimals);
            if (_stolenAnimals >= _targetAmountOfStolenAnimals)
            {
                _collider.enabled = false;
                AnimalsAlreadyStolen?.Invoke();
            }
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
        if (_stolenAnimals >= _targetAmountOfStolenAnimals)
        {
            return;
        }
        
        animal.gameObject.TryGetComponent(out AnimalMovement movement);
        movement.enabled = false;
        animal.gameObject.transform.position = _stolenAnimalsPosition.position;
        _stolenAnimals++;
    }
}

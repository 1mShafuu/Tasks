using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    private const float ChangeTargetMovementTime = 10f;
    private const float MaxDegreeDelta = 90f;
    
    private Vector3 _targetMovement;
    private Animator _animator;
    private float _speed = 1.5f;
    private float _minDistance = 2f;
    private float _elapsedTime = 0;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GetNewTargetPosition();
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        
        if (_elapsedTime >= ChangeTargetMovementTime)
        {
            GetNewTargetPosition();
            _elapsedTime = 0;
        }
        
        if (Vector3.Distance(transform.position, _targetMovement) >= _minDistance)
        {
            _animator.Play("Locomotion");
            transform.position = Vector3.MoveTowards(transform.position, _targetMovement, _speed * Time.deltaTime);
            //transform.LookAt(_targetMovement);
        }
        else
        {
            _animator.Play("Eat");
        }
    }

    private void GetNewTargetPosition()
    {
        const int radius = 10;
        Vector3 newTargetPosition = Random.insideUnitSphere * radius + transform.position;
        newTargetPosition.y = 0;
        _targetMovement = newTargetPosition;
    }
    
}
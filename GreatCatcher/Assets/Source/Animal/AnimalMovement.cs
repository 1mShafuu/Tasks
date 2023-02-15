using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    private const float MinDistance = 2f;
    private const float ChangeTargetMovementTime = 25f;

    private Vector3 _targetMovement;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private float _elapsedTime = 0;
    private float _rotationSpeed = 2f;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
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
        
        if (Vector3.Distance(transform.position, _targetMovement) >= MinDistance)
        {
            _animator.Play("Locomotion");
            Vector3 moveTo = transform.position + _targetMovement * Time.deltaTime;
            moveTo.y = 0;
            _rigidbody.MovePosition(moveTo);
            Quaternion targetRotation = Quaternion.LookRotation(_targetMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        else
        {
            _animator.Play("Eat");
        }
    }

    private void GetNewTargetPosition()
    {
        Vector3 newTargetPosition = Random.insideUnitSphere;
        newTargetPosition.y = 0;
        _targetMovement = newTargetPosition.normalized;
    }
    
}
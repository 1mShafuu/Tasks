using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    private const float ChangeTargetMovementTime = 10f;
    
    private Vector3 _targetMovement;
    private float _speed = 1.5f;
    private float minDistance = 1f;
    private float _elapsedTime = 0;
    
    private void Awake()
    {
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
        
        if (Vector3.Distance(transform.position, _targetMovement) >= minDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetMovement, _speed * Time.deltaTime);
        }
    }

    private void GetNewTargetPosition()
    {
        Vector3 newTargetPosition = Random.insideUnitSphere * 10 + transform.position;
        newTargetPosition.y = 0;
        _targetMovement = newTargetPosition;
    }
    
}
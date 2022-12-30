using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    private Vector3 _targetMovement;
    private float _speed = 1.5f;
    private float minDistance = 1f;
    
    private void Awake()
    {
        GetNewTargetPosition();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _targetMovement) >= minDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetMovement, _speed * Time.deltaTime);
            
        }
        else
        {
            GetNewTargetPosition();
        }
    }

    private void GetNewTargetPosition()
    {
        Vector3 newTargetPosition = Random.insideUnitSphere * 10 + transform.position;
        newTargetPosition.y = 0;
        _targetMovement = newTargetPosition;
    }
    
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsThiefMovement : MonoBehaviour
{
    private const float MinDistance = 2f;
    
    [SerializeField] private Transform _startPostion;
    [SerializeField] private Transform _endPosition;
    
    private AnimalsThief _thief;
    private Vector3 _targetMovement;
    private float _speed = 5f;

    public event Action EndPositionReached;
    
    private void Awake()
    {
        _thief = GetComponent<AnimalsThief>();
    }

    private void OnEnable()
    {
        _thief.AnimalsAlreadyStolen += OnAnimalsAlreadyStolen;
        _targetMovement = (_thief.TargetMovement - transform.position).normalized;
    }

    private void OnDisable()
    {
        _thief.AnimalsAlreadyStolen -= OnAnimalsAlreadyStolen;
        transform.position = _startPostion.position;
    }
    
    private void Update()
    {
        transform.Translate(_targetMovement * (Time.deltaTime * _speed));

        if (Vector3.Distance(transform.position, _endPosition.position) < MinDistance)
        {
            EndPositionReached?.Invoke();
        }
    }


    private void OnAnimalsAlreadyStolen()
    {
        _targetMovement = (_endPosition.position - transform.position).normalized;
    }
}

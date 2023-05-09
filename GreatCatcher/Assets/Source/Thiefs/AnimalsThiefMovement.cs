using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsThiefMovement : MonoBehaviour
{
    [SerializeField] private Transform _startPostion;
    [SerializeField] private Transform _endPosition;
    [SerializeField] private ThiefDetector _thiefDetector;
    
    private AnimalsThief _thief;
    private Vector3 _targetMovement;
    private float _speed = 7f;

    public event Action EndPositionReached;
    public event Action MovementStarted;
    public event Action MovementTargetChanged;
    
    private void Awake()
    {
        _thief = GetComponent<AnimalsThief>();
    }

    private void OnEnable()
    {
        _thief.AnimalsAlreadyStolen += OnAnimalsAlreadyStolen;
        _thiefDetector.ThiefDetected += OnAnimalsAlreadyStolen;
        _targetMovement = (_thief.TargetMovement - transform.position).normalized;
        MovementStarted?.Invoke();
    }

    private void OnDisable()
    {
        _thief.AnimalsAlreadyStolen -= OnAnimalsAlreadyStolen;
        _thiefDetector.ThiefDetected -= OnAnimalsAlreadyStolen;
        transform.position = _startPostion.position;
    }
    
    private void Update()
    {
        transform.Translate(_targetMovement * (Time.deltaTime * _speed));
        //Debug.Log(Vector3.Distance(transform.position, _endPosition.position));
        
        if (Vector3.Distance(transform.position, _endPosition.position) < AnimalsThief.MinDistance)
        {
            EndPositionReached?.Invoke();
        }
    }

    private void OnAnimalsAlreadyStolen()
    {
        ChangeTargetMovementToEndPoint();
    }
    
    private void ChangeTargetMovementToEndPoint()
    {
        _targetMovement = (_endPosition.position - transform.position).normalized;
        MovementTargetChanged?.Invoke();
    }
}

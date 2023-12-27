using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointerToSheriff : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private ThiefAutomation _thiefAutomation;
    [SerializeField] private ArrowThief _arrowThief;
    [SerializeField] private AnimalsThiefMovement _animalsThiefMovement;
    
    private Vector3 _initialDirection = new(-1f, 0f, 0f);
    private bool _isArrowEnabled;

    private void OnEnable()
    {
        _thiefAutomation.MovementEnabled += OnMovementStarted;
        _animalsThiefMovement.MovementTargetChanged += OnMovementTargetChanged;
    }

    private void OnDisable()
    {
        _thiefAutomation.MovementEnabled -= OnMovementStarted;
        _animalsThiefMovement.MovementTargetChanged -= OnMovementTargetChanged;
    }

    private void Start()
    {
        _arrowThief.transform.rotation = Quaternion.LookRotation(_initialDirection);
        _isArrowEnabled = false;
        _arrowThief.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_targetPoint == null)
        {
            Debug.LogWarning("Target point is not assigned.");
            return;
        }
        
        Vector3 directionToTarget = _targetPoint.position - _arrowThief.transform.position;

        if (directionToTarget == Vector3.zero) return;
        
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            
        targetRotation *= Quaternion.Euler(0, 90, 0);
            
        _arrowThief.transform.rotation = Quaternion.Euler(new Vector3(_arrowThief.transform.rotation.eulerAngles.x, targetRotation.eulerAngles.y, _arrowThief.transform.rotation.eulerAngles.z));
    }

    private void OnMovementStarted()
    {
        if (_isArrowEnabled) return;
        
//        Debug.Log("OnMovementStarted");
        _arrowThief.gameObject.SetActive(true);
        _isArrowEnabled = true;
    }

    private void OnMovementTargetChanged()
    {
        if (!_isArrowEnabled) return;
        
        _arrowThief.gameObject.SetActive(false);
        _isArrowEnabled = false;
    }
}

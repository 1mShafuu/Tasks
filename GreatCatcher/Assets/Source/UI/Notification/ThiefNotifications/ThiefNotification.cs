using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefNotification : MonoBehaviour
{
    [SerializeField] private AnimalsThiefMovement _animalsThiefMovement;

    private void OnEnable()
    {
        _animalsThiefMovement.MovementStarted += OnMovementStarted;
        _animalsThiefMovement.MovementTargetChanged += OnMovementTargetChanged;
    }

    private void OnDisable()
    {
        _animalsThiefMovement.MovementStarted -= OnMovementStarted;
        _animalsThiefMovement.MovementTargetChanged -= OnMovementTargetChanged;
    }

    private void Start()
    {
        transform.localScale = Vector2.zero;
    }
    
    private void Open()
    {
        transform.LeanScale(Vector2.one, 0.8f);
    }

    private void Close()
    {
        transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }

    private void OnMovementStarted()
    {
        Open();
    }

    private void OnMovementTargetChanged()
    {
        Close();
    }
}

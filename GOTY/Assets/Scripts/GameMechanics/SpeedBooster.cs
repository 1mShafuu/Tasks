using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedBooster : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    
    private float _speed;
    private float _speedIncrease = 0.5f;

    public float Speed => _speed;
    public float StartSpeed { get; private set; }

    public event UnityAction<float> SpeedChanged;

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        _speed = _movement.Speed;
        StartSpeed = _speed;
    }

    public void SpeedChange()
    {
        _speed +=_speedIncrease;
        SpeedChanged?.Invoke(_speed);
    }
}

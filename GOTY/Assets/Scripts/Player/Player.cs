using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private Vector3 _startPosition;

    private const int CheckPoint = 50;
    private const int StartHP = 1;

    private SpeedBooster _booster;
    private int _score;
    private int _checkpointDistance;
    private float _startPositionCoordinateX;

    public Vector3 StartPosition => _startPosition;
    public int TravelledDistance { get; private set; }
    
    public event UnityAction GameOver;
    public event UnityAction<int> ScoreChanged;

    private void Awake()
    {
        _startPosition = transform.position;
        _booster = GetComponent<SpeedBooster>();
    }

    private void Start()
    {
        _startPositionCoordinateX = transform.position.x;
        _checkpointDistance = CheckPoint;
    }

    private void Update()
    {
        CheckScore();
        MeasureTravelledDistance();
    }

    public void CheckScore()
    {
        _score = TravelledDistance;
        ScoreChanged?.Invoke(_score);
    }
    
    public void Die()
    {
        Debug.Log("DEEEEAAAAD");
        Time.timeScale = 0;
        GameOver?.Invoke();
    }

    public void Reset()
    {
        _score = 0;
        _health = StartHP;
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void MeasureTravelledDistance()
    {
        var currentDistance = (transform.position.x - _startPositionCoordinateX);
        TravelledDistance = (int)currentDistance;

        if ((int)currentDistance == _checkpointDistance)
        {
            _booster.SpeedChange();
            _checkpointDistance += CheckPoint;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BirdMover : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private float _tapForce = 10f;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _maxRotationZ;
    [SerializeField] private float _minRotationZ;

    private float _speed = 2;
    private Rigidbody2D _rigidbody2D;
    private Quaternion _maxRotation;
    private Quaternion _minRotation;
    private float _startX;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _maxRotation = Quaternion.Euler(0 , 0, _maxRotationZ);
        _minRotation = Quaternion.Euler(0, 0, _minRotationZ);
        _startX = transform.position.x;
        ResetBird();
    }

    private void Update()
    {
        var stepsToIncreaseSpeed = 50;
        var speedIncreaser = 0.5f;
        var distanceTraveled = transform.position.x - _startX;
        Debug.Log(distanceTraveled);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody2D.velocity = new Vector2(_speed, 0);
            transform.rotation = _maxRotation;
            _rigidbody2D.AddForce(Vector2.up * _tapForce, ForceMode2D.Force);
        }

        /*if ((int)(distanceTraveled / 10) == 1)
        {
            _speed += speedIncreaser;
            _startX = transform.position.x;
        }*/
        
        transform.rotation = Quaternion.Lerp(transform.rotation, _minRotation, _rotationSpeed * Time.deltaTime);
    }

    public void ResetBird()
    {
        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        _rigidbody2D.velocity = Vector2.zero;
    }
}

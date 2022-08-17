using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private string _playerTag;
    [SerializeField] private float _movingSpeed;

    private int _numberToCenter = 10;
    
    private void Awake()
    {
        transform.position = new Vector3()
        {
            x = _playerTransform.position.x,
            y = _playerTransform.position.y,
            z = _playerTransform.position.z - _numberToCenter
        };
    }

    private void Update()
    {
        if (_playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = _playerTransform.position.x,
                y = _playerTransform.position.y,
                z = _playerTransform.position.z - _numberToCenter
            };
            Vector3 targetPosition = Vector3.Lerp(transform.position, target, _movingSpeed * Time.deltaTime);
            transform.position = targetPosition;
        }
    }
}

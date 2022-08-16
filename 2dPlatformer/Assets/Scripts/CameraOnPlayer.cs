using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private string _playerTag;
    [SerializeField] private float _movingSpeed;

    private void Awake()
    {
        if (this._playerTransform == null)
        {
            if (_playerTag == "")
            {
                _playerTag = "Player";
            }
            
            this._playerTransform = GameObject.FindGameObjectWithTag(this._playerTag).transform;
        }

        this.transform.position = new Vector3()
        {
            x = this._playerTransform.position.x,
            y = this._playerTransform.position.y,
            z = this._playerTransform.position.z - 10
        };
        
    }

    private void Update()
    {
        if (this._playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = this._playerTransform.position.x,
                y = this._playerTransform.position.y,
                z = this._playerTransform.position.z - 10
            };

            Vector3 targetPosition = Vector3.Lerp(transform.position, target, _movingSpeed * Time.deltaTime);

            this.transform.position = targetPosition;
        }
    }
}

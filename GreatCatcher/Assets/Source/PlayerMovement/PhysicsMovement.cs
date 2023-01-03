using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SurfaceSlider))]
public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;
    private SurfaceSlider _surfaceSlider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _surfaceSlider = GetComponent<SurfaceSlider>();
    }

    public void Move(Vector3 direction)
    {
        Vector3 directionAlongSurface = _surfaceSlider.Project(direction.normalized);
       // Debug.Log(directionAlongSurface);
        Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);
        Vector3 newVectorPosition = _rigidbody.position + offset;
        
        _rigidbody.MovePosition(newVectorPosition);
    }
}

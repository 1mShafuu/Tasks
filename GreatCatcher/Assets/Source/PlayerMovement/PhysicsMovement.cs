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
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _surfaceSlider = GetComponent<SurfaceSlider>();
    }

    public void Move(Vector3 direction)
    {
        const float maxDegreeDelta = 1f;
        
        if (direction != Vector3.zero)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _animator.Play("Run");
            }
            
            Quaternion toRotation = Quaternion.LookRotation(direction,Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation,maxDegreeDelta);
        }
        else
        {
            _animator.Play("Idle");
        }
        
        Vector3 directionAlongSurface = _surfaceSlider.Project(direction.normalized);
       // Debug.Log(directionAlongSurface);
        Vector3 offset = directionAlongSurface * (_speed * Time.deltaTime);
        Vector3 newVectorPosition = _rigidbody.position + offset;
        
        _rigidbody.MovePosition(newVectorPosition);
    }
}

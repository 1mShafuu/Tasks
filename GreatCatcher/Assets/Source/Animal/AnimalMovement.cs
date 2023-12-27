using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalMovement : MonoBehaviour
{
    private const float MinDistanceMultiplierMin = 0.2f;
    private const float MinDistanceMultiplierMax = 0.6f;
    private const float SpawnRadius = 85f;
    private const float ChangeTargetMovementTime = 25f;
    private const float ChangeDirectionDistance = 140f;
    private const float RotationSpeed = 2f;
    private const float MinDistanceOffset = 6f;
    private const float CaughtAnimalMovementSpeed = 12f;

    private float _movementSpeed = 2f;
    private float _minDistance = 5f;
    private Vector3 _spawnPosition;
    private Vector3 _targetMovement;
    private Animator _animator;
    private Animal _animal;
    private Rigidbody _rigidbody;
    private float _elapsedTime = 0f;
    private float _totalDistanceTraveled = 0f;
    private float _distanceMultiplier;
    private Player _player;
    private GameObject _playersGameObject;
    private RaycastHit _hitInfo;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _spawnPosition = transform.position;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _animal = GetComponent<Animal>();
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _playersGameObject = _player.gameObject;
        ChangeTargetPosition();
    }

    private void FixedUpdate()
    {
        UpdateTargetPosition();
        HandleMovement();
    }

    private void UpdateTargetPosition()
    {
        if (_animal.IsCaught)
        {
            _targetMovement = _playersGameObject.transform.position;
            _minDistance = MinDistanceOffset;
            _movementSpeed = CaughtAnimalMovementSpeed;
        }
        else if (_elapsedTime >= ChangeTargetMovementTime)
        {
            ChangeTargetPosition();
            _elapsedTime = 0f;
            _totalDistanceTraveled = 0f;
        }
    }

    private void HandleMovement()
    {
        _elapsedTime += Time.deltaTime;

        if ((_rigidbody.position - _targetMovement).magnitude >= _minDistance)
        {
            if (IsGrounded())
            {
                if (CheckWallInFront())
                {
                    ReflectFromWall(_hitInfo);
                    //Debug.Log($"{(_rigidbody.position - _targetMovement).magnitude} {_minDistance}  {_hitInfo.collider.name}");
                }
                else
                {
                    MoveTowardsTarget();
                }
            }
            else
            {
                ChangeTargetPosition();
            }
        }
        else
        {
            _animator.Play("Eat");
        }
    }


    private bool CheckWallInFront()
    {
        const float wallCheckDistance = 1.0f;
        var direction = transform.forward;

        //Debug.DrawRay(transform.position + (direction + Vector3.up) * 0.7f, direction * wallCheckDistance, Color.blue);

        if (Physics.Raycast(transform.position + (direction + Vector3.up) * 0.7f, direction.normalized, out var newHitInfo, wallCheckDistance))
        {
            if (newHitInfo.collider.isTrigger == false && !newHitInfo.collider.TryGetComponent(out InvisibleWallToIgnore wall))
            {
                _hitInfo = newHitInfo;
                return true;
            }
        }

        return false;
    }

    
    private bool IsGrounded()
    {
        const float groundCheckDistance = .3f;
        const float heightMultiplier = 0.2f;
        var raycastOrigin = _rigidbody.position + Vector3.up * heightMultiplier; 
        
        return Physics.Raycast(raycastOrigin, Vector3.down, out RaycastHit hit, groundCheckDistance);
    }

    private void ReflectFromWall(RaycastHit hitInfo)
    {
        Vector3 wallNormal = hitInfo.normal;
        Vector3 reflectedDirection = Vector3.Reflect(_targetMovement - transform.position, wallNormal);

        _targetMovement = hitInfo.point + 
                          reflectedDirection.normalized * Mathf.Max((_targetMovement - transform.position).magnitude, MinDistanceOffset);

        Quaternion targetRotation = Quaternion.LookRotation(reflectedDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
    }
    
    private void MoveTowardsTarget()
    {
        _animator.Play("Locomotion");
        var newPosition = Vector3.MoveTowards(_rigidbody.position, _targetMovement, Time.deltaTime * _movementSpeed);
        _rigidbody.MovePosition(newPosition);
        var targetRotation = Quaternion.LookRotation(_targetMovement - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        _totalDistanceTraveled += (_rigidbody.position - _targetMovement).magnitude;
    }
    
    private void ChangeTargetPosition()
    {
        var randomDirection = Random.insideUnitSphere * ChangeDirectionDistance;
        randomDirection.y = 0f;
        var randomCirclePoint = Random.insideUnitCircle.normalized * SpawnRadius;
        var randomOffset = new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        _targetMovement = _spawnPosition + randomOffset + randomDirection;
        GetNewMinDistanceMultiplier();
    }
    
    private void GetNewMinDistanceMultiplier()
    {
        _distanceMultiplier = Random.Range(MinDistanceMultiplierMin, MinDistanceMultiplierMax);
        _minDistance = (_rigidbody.position - _targetMovement).magnitude * _distanceMultiplier;
    }
}

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MovementBorders))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForceUp;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _jumpCooldown;
    
    private const float JumpForceDown = -10;
    private const float LineDistance = 4f;

    private bool _isGrounded;
    private bool _isReadyToJump;
    private RaycastHit _hit;
    private SpeedBooster _speedBooster;
    private MovementBorders _borders;
    private float _leftBorder;
    private float _rightBorder;
    private int _moveLine = 1;

    public float Speed => _speed;

    private void Awake()
    {
        _isReadyToJump = true;
        _rigidbody = GetComponent<Rigidbody>();
        _speedBooster = GetComponent<SpeedBooster>();
        _borders = gameObject.AddComponent<MovementBorders>();
    }

    private void Start()
    {
        _leftBorder = _borders.LeftBorder;
        _rightBorder = _borders.RightBorder;
        transform.position = _borders.CenterPosition.position + Vector3.up;
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 2 * 0.5f + 0.3f, _whatIsGround);
    }

    private void OnEnable()
    {
        _speedBooster.SpeedChanged += OnSpeedChanged;
    }

    private void OnDisable()
    {
        _speedBooster.SpeedChanged -= OnSpeedChanged;
    }
    
    public void Move(bool keyDownLeft, bool keyDownRight)
    {
        if (keyDownRight)
        {
            if (_moveLine < 2)
            {
                _moveLine++;
                _rigidbody.position += Vector3.back * LineDistance;
            }
        }

        if (keyDownLeft)
        {
            if (_moveLine > 0)
            { 
                _moveLine--; 
                _rigidbody.position += Vector3.forward * LineDistance;
            }
        }
        
        _rigidbody.MovePosition(_rigidbody.position + Vector3.right * (_speed * Time.deltaTime));
    }

    public void JumpUp()
    {
        if(_isReadyToJump && _isGrounded)
        {
            _isReadyToJump = false;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
            _rigidbody.AddForce(transform.up * _jumpForceUp, ForceMode.Impulse);
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    public void JumpDown()
    {
        if (_isGrounded == false)
        {
            _rigidbody.AddForce(transform.up * JumpForceDown, ForceMode.VelocityChange);
            _jumpCooldown = 0;
        }
    }
    
    private void ResetJump()
    {
        _isReadyToJump = true;
    }

    private void OnSpeedChanged(float speed)
    {
        _speed = speed;
    }
}

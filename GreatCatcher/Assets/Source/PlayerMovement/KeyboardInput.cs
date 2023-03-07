using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;

    private Animator _animator;
    private Vector2 _moveInput;
    private PlayerInput _playerInput;
    private InputAction _touchscreen;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Idle");
        _playerInput = new PlayerInput();
        _touchscreen = new InputAction("TouchAction", InputActionType.PassThrough, "<touchscreen>/touch*/position");
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _touchscreen.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _touchscreen.Disable();
    }

    private void Update()
    {
        // if (_touchscreen.ReadValue<Vector2>() != Vector2.zero)
        // {
        //     
        // }
        // else
        // {
        //     _moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
        // }
        // float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        _moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
        Vector3 movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        //transform.forward = movement;
        //_characterController.Move(movement * (Time.deltaTime * 7f));
        //_movement.Move(new Vector3(horizontal, 0, vertical));
        _movement.Move(movementDirection);
        
    }
}

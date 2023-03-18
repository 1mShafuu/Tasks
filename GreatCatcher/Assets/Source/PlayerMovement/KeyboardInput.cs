using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;
    [SerializeField] private UltimateJoystick _joystick;
    [SerializeField] private Tutorial _tutorial;
    
    private Animator _animator;
    private Vector2 _moveInput;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Idle");
        _playerInput = new PlayerInput();
        _joystick.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _tutorial.TutorialEnded += OnTutorialEnded;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _tutorial.TutorialEnded -= OnTutorialEnded;
    }

    private void Update()
    {
        
        if(Application.isMobilePlatform)
        {
            float horizontal = UltimateJoystick.GetHorizontalAxis("Movement");
            float vertical = UltimateJoystick.GetVerticalAxis("Movement");
            Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
            _movement.Move(movementDirection);
        }
        else
        {
            _moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            _movement.Move(movementDirection);
        }
    }

    private void OnTutorialEnded()
    { 
        if (Application.isMobilePlatform)
        {
            _joystick.gameObject.SetActive(true);
        }
    }
}

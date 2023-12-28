using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _movement;
    [SerializeField] private UltimateJoystick _joystick;
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private JoystickSwitcher _joystickSwitcher;
    
    private Animator _animator;
    private Vector2 _moveInput;
    private PlayerInput _playerInput;

    public event Action PlayerStayed;
    public event Action<Vector3> MovementVectorChanged;
    
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
        float horizontal = 0f;
        float vertical = 0f;
        
        if (!_joystick.isActiveAndEnabled)
        {
            Vector2 moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            horizontal = moveInput.x;
            vertical = moveInput.y;
        }
        else
        {
            var moveInput = _playerInput.Player.Move.ReadValue<Vector2>();
            var horizontalJoystick = UltimateJoystick.GetHorizontalAxis("Movement");
            var verticalJoystick  = UltimateJoystick.GetVerticalAxis("Movement");
            
            if (moveInput.x != 0 || moveInput.y != 0)
            {
                horizontal = moveInput.x;
                vertical = moveInput.y;
            }
            else if(horizontalJoystick != 0 || verticalJoystick != 0)
            {
                horizontal = horizontalJoystick;
                vertical = verticalJoystick;
            }
        }
        
        var movementDirection = new Vector3(horizontal, 0, vertical);

        if (movementDirection != Vector3.zero)
        {
            //Debug.Log(movementDirection);
        }
        
        MovementVectorChanged?.Invoke(movementDirection);

        if (movementDirection == Vector3.zero)
        {
            
            PlayerStayed?.Invoke();
        }
        
        _movement.Move(movementDirection);
    }

    private void OnTutorialEnded()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Agava.WebUtility.Device.IsMobile || _joystickSwitcher.JoystickChosen) // Application.isMobilePlatform
        {
            _joystick.gameObject.SetActive(true);
        }
#else
        if (_joystickSwitcher.JoystickChosen) // Application.isMobilePlatform
        {
            _joystick.gameObject.SetActive(true);
        }
#endif
    }
}

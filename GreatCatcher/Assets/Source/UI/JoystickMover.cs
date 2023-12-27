using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class JoystickMover : MonoBehaviour
{
    private const int ScreenHeightLimitFactor = 300;
    
    [SerializeField] private UltimateJoystick _joystick;
    [SerializeField] private RectTransform _joystickBase;
    [SerializeField] private KeyboardInput _input;
    
    private readonly Vector3 _offsetVector = new Vector3(-120, -120);
    private Vector2 _mousePosition;
    private Vector2 _startPosition;
    private Vector2 _initialMousePosition;
    private bool _isJoystickAbleToMove = true;
    private bool _isTouchingJoystick = false; 

    private void OnEnable()
    {
        _input.PlayerStayed += OnPlayerStayed;
    }

    private void OnDisable()
    {
        _input.PlayerStayed -= OnPlayerStayed;
    }

    private void Start()
    {
        _initialMousePosition = Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        _joystickBase.position = _joystick.gameObject.transform.position + _offsetVector;
    }

    private void OnPlayerStayed()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            _mousePosition = Mouse.current.position.ReadValue();

            if (_isTouchingJoystick)
            {
                _joystick.transform.position = _mousePosition;
            }
            else if (_mousePosition != _initialMousePosition)
            {
                Vector3 newPosition = _mousePosition;
                newPosition.z = 0f;

                var screenWidth = Screen.width;
                var screenHeight = Screen.height - ScreenHeightLimitFactor;
                
                newPosition.x = Mathf.Clamp(newPosition.x, 0f, screenWidth);
                newPosition.y = Mathf.Clamp(newPosition.y, 0f, screenHeight);
                _joystick.transform.position = newPosition;
                _initialMousePosition = _mousePosition;
            }
        }
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        _isTouchingJoystick = true;
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        _isTouchingJoystick = false;
    }
}

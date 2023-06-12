using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoystickMover : MonoBehaviour
{
    private readonly Vector3 _offsetVector = new Vector3(-120, -120);

    [SerializeField] private UltimateJoystick _joystick;
    [SerializeField] private RectTransform _joystickBase;
    [SerializeField] private KeyboardInput _input;
    
    private Vector2 _mousePosition;
    private Vector2 _startPosition;
    private Vector2 _initialMousePosition;
    private bool _isJoystickAbleToMove = true;

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

            // Проверяем, что мышь изменила свое положение относительно начальной позиции
            if (_mousePosition != _initialMousePosition)
            {
                Vector3 newPosition = _mousePosition;
                newPosition.z = 0f;

                int screenWidth = Screen.width;
                int screenHeight = Screen.height-300;

                // Ограничение позиции по x и y координатам
                newPosition.x = Mathf.Clamp(newPosition.x, 0f, screenWidth);
                newPosition.y = Mathf.Clamp(newPosition.y, 0f, screenHeight);
                Debug.Log(newPosition);
                _joystick.transform.position = newPosition;

                // Обновляем начальную позицию мыши после перемещения джойстика
                _initialMousePosition = _mousePosition;
            }
        }
    }
}

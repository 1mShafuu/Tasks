using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoystickNotification : Notification
{
    [SerializeField] private JoystickSwitcher _joystickSwitcher;

    private void OnEnable()
    {
        _joystickSwitcher.ModeSwitched += OnModeSwitched;
    }

    private void OnDisable()
    {
        _joystickSwitcher.ModeSwitched -= OnModeSwitched;
    }

    private void Start()
    {
        transform.localScale = Vector2.zero;
    }
    
    protected override void Open()
    {
        transform.LeanScale(Vector2.one, 0.8f);
    }

    protected override void Close()
    {
        transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }

    private void OnModeSwitched()
    {
        StartCoroutine(NotificationShown(this));
    }
}

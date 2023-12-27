using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitAnimalsNotification : Notification
{
    [SerializeField] private SellArea _sellArea;
    [SerializeField] private GameObject _message;

    private Coroutine _coroutine;
    
    private void Start()
    {
        _message.transform.localScale = Vector2.zero;
    }

    private void OnEnable()
    {
        _sellArea.AnimalsLimitReached += OnAnimalsLimitReached;
    }

    private void OnDisable()
    {
        _sellArea.AnimalsLimitReached -= OnAnimalsLimitReached;
    }

    protected override void Open()
    {
        _message.transform.LeanScale(Vector2.one, 0.8f);
    }

    protected override void Close()
    {
        _message.transform.LeanScale(Vector2.zero, 1f).setEaseInBack();
    }
    
    private void OnAnimalsLimitReached(int value)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(NotificationShown(this));
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellAnimalsNotification : Notification
{
    [SerializeField] private AnimalSeller _animalSeller;

    private Coroutine _coroutine;
    
    private void OnEnable()
    {
        _animalSeller.NotAbleToSellNotified += OnNotAbleToSellNotified;
    }

    private void OnDisable()
    {
        _animalSeller.NotAbleToSellNotified -= OnNotAbleToSellNotified;
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

    private void OnNotAbleToSellNotified()
    {
      //  Debug.Log("SellAnimalsNotification");
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
      
        _coroutine = StartCoroutine(NotificationShown(this));
    }
}

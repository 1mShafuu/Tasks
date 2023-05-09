using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitAnimalsNotification : MonoBehaviour
{
    [SerializeField] private SellArea _sellArea;
    [SerializeField] private GameObject _message;
    
    private float _activeSeconds = 5f;

    private void Start()
    {
        _message.SetActive(false);
    }

    private void OnEnable()
    {
        _sellArea.AnimalsLimitReached += OnAnimalsLimitReached;
    }

    private void OnDisable()
    {
        _sellArea.AnimalsLimitReached -= OnAnimalsLimitReached;
    }

    private void OnAnimalsLimitReached()
    {
        StartCoroutine(ToggleObject());
    }

    private IEnumerator ToggleObject()
    {
        var waitForSeconds = new WaitForSeconds(_activeSeconds);
        _message.SetActive(true);
        yield return waitForSeconds;
        _message.SetActive(false);
    }
}

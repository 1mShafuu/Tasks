using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider), typeof(Player))]
public class HealthUI : MonoBehaviour
{
    private const float СhangerSpeed = 9f;
    
    [SerializeField] private Slider _slider;
    [SerializeField] private Player _player;

    private float _changedSliderValue;
    private Coroutine _healthChanger;
    
    private void Awake()
    {
        _player.HealthChanged += HealthChange;
        _changedSliderValue = _player.Health;
         _slider.value = _player.Health;
    }

    public void HealthChange()
    {
        if (_healthChanger != null)
        {
            StopCoroutine(_healthChanger);
        }

        _healthChanger = StartCoroutine(HealthChanger());
    }
    
    private IEnumerator HealthChanger()
    {
        const float seconds = 0.01f;
        var waitForSeconds = new WaitForSeconds(seconds);
        _changedSliderValue = _player.Health;

        while (_slider.value != _changedSliderValue)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _changedSliderValue,  СhangerSpeed * Time.deltaTime);
            yield return waitForSeconds;
        }
    }
}

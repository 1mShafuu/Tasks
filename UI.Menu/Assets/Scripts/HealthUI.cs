using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider), typeof(Player))]
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Player _player;
    
    private const int maxHealthToHeal = 90;
    private const int minHealthToDamage = 10;
    private const float _changeSpeed = 5f;

    private float _changedSliderValue;

    private void Awake()
    {
         _changedSliderValue = _player.Health;
    }

    private void Update()
    {
        
        _slider.value = Mathf.MoveTowards(_slider.value, _changedSliderValue,  _changeSpeed * Time.deltaTime);
    }

    public void Heal()
    {
        if (_player.Health <= maxHealthToHeal)
        {
            _player.TakeHeal();
            _changedSliderValue = _player.Health;
        }
    }

    public void Damage()
    {
        if (_player.Health >= minHealthToDamage)
        {
             _player.TakeDamage();
             _changedSliderValue = _player.Health;
        }
    }
}

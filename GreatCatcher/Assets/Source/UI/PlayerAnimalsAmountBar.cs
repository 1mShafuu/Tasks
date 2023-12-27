using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimalsAmountBar : MonoBehaviour
{
    [SerializeField] private SellArea _sellArea;
    [SerializeField] private TMP_Text _text;
    
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _sellArea.MaxAmountOfAnimalsInAviary;
    }
    
    private void OnEnable()
    {
        _sellArea.AnimalsLimitReached += OnAnimalsLimitReached;
        _sellArea.AnimalsLimitNotReached += OnAnimalsLimitReached;
        _sellArea.MaxAmountOfAnimalsChanged += OnMaxAmountOfAnimalsChanged;
        _slider.value = 0;
    }

    private void OnDisable()
    {
        _sellArea.AnimalsLimitReached -= OnAnimalsLimitReached;
        _sellArea.AnimalsLimitNotReached -= OnAnimalsLimitReached;
        _sellArea.MaxAmountOfAnimalsChanged -= OnMaxAmountOfAnimalsChanged;
    }

    private void OnAnimalsLimitReached(int value)
    {
        if (value >= _slider.maxValue)
        {
            _slider.value = _slider.maxValue;
        }
        else
        {
            _slider.value = value;
        }

        _text.text = Convert.ToString(_slider.value, CultureInfo.InvariantCulture);
    }
    
    private void OnMaxAmountOfAnimalsChanged()
    {
        _slider.maxValue = _sellArea.MaxAmountOfAnimalsInAviary;
    }
}

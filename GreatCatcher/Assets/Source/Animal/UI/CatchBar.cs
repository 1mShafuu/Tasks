using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatchBar : MonoBehaviour
{
    private Canvas _canvas;
    private Slider _slider;
    private UIContainer _container;
    
    private void Awake()
    {
        _container = GetComponentInParent<UIContainer>();
        _canvas = GetComponent<Canvas>();
        _slider = _canvas.GetComponentInChildren<Slider>();
        _canvas.enabled = false;
    }

    private void OnEnable()
    {
        _container.AnimalDiscovered += OnAnimalDiscovered;
    }

    private void OnDisable()
    {
        _container.AnimalDiscovered -= OnAnimalDiscovered;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    public void TurnOffCanvas()
    {
        _canvas.enabled = false;
    }
    
    private void ValueChange(float value, float maxValue)
    {
        float uncertainty = 0.0000001f;
        
        if (Math.Abs(value - maxValue) < uncertainty)
        {
            value = 0;
        }
        
        _slider.value = value;
    }

    private void OnAnimalDiscovered(GameObject animal)
    {
        animal.TryGetComponent(out CatchArea area);
        _canvas.enabled = true;
        ValueChange(area.ElapsedTime, area.TimeToCatch);
    }
}

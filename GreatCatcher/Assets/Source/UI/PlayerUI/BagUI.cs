using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BagUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentCatchedAmount;
    [SerializeField] private TMP_Text _notification;
    
    private Bag _bag;
    private Canvas _canvas;
    private float _rotationSpeed = 2000f;
    private Camera _mainCamera;
    private float _canvasPercentage = 5f;

    private void Awake()
    {
        _bag = GetComponentInParent<Bag>();
        _canvas = GetComponent<Canvas>();
        _notification.alpha = 0;
        _mainCamera = Camera.main; 
    }

    private void OnEnable()
    {
        _bag.AnimalsAmountChanged += OnAnimalsAmountChanged;
    }

    private void OnDisable()
    {
        _bag.AnimalsAmountChanged -= OnAnimalsAmountChanged;
    }
    
    private void Update()
    {
         var rotationGoal = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
         transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal,_rotationSpeed * Time.deltaTime);
    }
    
    private void OnAnimalsAmountChanged(int value)
    {
        _currentCatchedAmount.text = value.ToString();

        if (value == _bag.MaxAmountOfAnimalsInBag)
        {
            _notification.alpha = 1;
        }
        else
        {
            _notification.alpha = 0;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BagUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentCatchedAmount;
    [SerializeField] private TMP_Text _notification;
    [SerializeField] private Player _player;
    
    private Bag _bag;
    private Camera _mainCamera;
    private float _rotationSpeed = 2000f;

    private void Awake()
    {
        _bag = _player.GetComponent<Bag>();
        _notification.alpha = 0;
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _bag.AnimalsAmountChanged += OnAnimalsAmountChanged;
        //_keyboardInput.MovementVectorChanged += OnMovementVectorChanged;
    }

    private void OnDisable()
    {
        _bag.AnimalsAmountChanged -= OnAnimalsAmountChanged;
       // _keyboardInput.MovementVectorChanged -= OnMovementVectorChanged;
    }

    private void Update()
    {
        var rotationGoal = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
        //transform.localRotation = rotationGoal;
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

    /*private void OnMovementVectorChanged(Vector3 movementVector)
    {
        if (movementVector != Vector3.zero)
        {
  
            float zValue = movementVector.x != 0 ? Mathf.Approximately(movementVector.z, 0f) ? 0 : Mathf.Approximately(movementVector.z, movementVector.x) ? -movementVector.x : movementVector.x  : -movementVector.z;
            
            Vector3 newPosition = new Vector3(movementVector.x, 0.5f, zValue);
            Debug.Log($"{movementVector} {zValue} Новая позиция : {newPosition}");
            
            transform.localPosition = newPosition;
        }
    }*/
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlideButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private float _startCoordinateY;
    private bool _isActiveMenu = false;

    private void Awake()
    {
        //_button = GetComponentInChildren<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClicked);
        _startCoordinateY = transform.position.y;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);
    }

    private void SlideDown()
    {
        LeanTween.moveLocalY(gameObject,380f, 0.5f);
    }

    private void SlideUp()
    {
        transform.LeanMoveY(_startCoordinateY, 1f).setEaseSpring();
    }

    private void OnButtonClicked()
    {
        if (_isActiveMenu)
        {
            SlideUp();
            _isActiveMenu = false;
        }
        else
        {
            SlideDown();
            _isActiveMenu = true;
        }
    }
}

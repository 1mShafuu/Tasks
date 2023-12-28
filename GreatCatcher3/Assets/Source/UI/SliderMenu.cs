using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMenu : MonoBehaviour
{
    private const float DistanceToMoveMenu = 250f;
    
    [SerializeField] private Button[] _subMenuButtons;
    [SerializeField] private Button _mainMenuButton;
    
    private float _startCoordinateY;
    private float _coordinateToMove;
    private bool _isActiveMenu = false;
    
    private void OnEnable()
    {
        _mainMenuButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _mainMenuButton.onClick.RemoveListener(OnButtonClicked);
    }

    private void SlideDown()
    {
        _startCoordinateY = transform.localPosition.y;
        _coordinateToMove = _startCoordinateY - DistanceToMoveMenu;
        LeanTween.moveLocalY(gameObject,_coordinateToMove, 0.7f);
    }

    private void SlideUp()
    {
        _startCoordinateY = transform.localPosition.y + DistanceToMoveMenu;
        LeanTween.moveLocalY(gameObject, _startCoordinateY, 0.5f).setEaseSpring();
    }

    private void OnButtonClicked()
    {
        if (_isActiveMenu)
        {
            SlideUp();
            _isActiveMenu = false;
            ShowHideButtons();
        }
        else
        {
            SlideDown();
            _isActiveMenu = true;
            ShowHideButtons();
        }
    }

    private void ShowHideButtons()
    {
        if (_isActiveMenu)
        {
            foreach (var button in _subMenuButtons)
            {
                button.TryGetComponent(out CanvasGroup canvasGroup);
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            foreach (var button in _subMenuButtons)
            {
                button.TryGetComponent(out CanvasGroup canvasGroup);
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
    }
}

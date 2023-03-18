using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SubStartScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _backToStartScreenButton;
    [SerializeField] private StartScreen _startScreen;
    
    private void OnEnable()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _backToStartScreenButton.onClick.AddListener(OnBackToStartScreen);
    }

    private void OnDisable()
    {
        _backToStartScreenButton.onClick.RemoveListener(OnBackToStartScreen);
    }

    public void Open()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _startScreen.Close();
    }

    public void TurnOffCanvasGroup()
    {
        _canvasGroup.alpha = 0;
    }
    
    private void OnBackToStartScreen()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _startScreen.Open();
    }
}

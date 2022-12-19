using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Screen : MonoBehaviour
{
    [SerializeField] protected CanvasGroup CanvasGroup;
    [SerializeField] protected Button RestartButton;
    [SerializeField] protected Button EndButton;
    
    private void OnEnable()
    {
        RestartButton.onClick.AddListener(OnRestartButtonClicked);
        EndButton.onClick.AddListener(OnEndButtonClicked);
    }

    private void OnDisable()
    {
        RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
        RestartButton.onClick.RemoveListener(OnEndButtonClicked);
    }

    protected abstract void OnRestartButtonClicked();

    public abstract void Open();
    
    public abstract void Close();

    private void OnEndButtonClicked()
    {
        Application.Quit();
    }
}

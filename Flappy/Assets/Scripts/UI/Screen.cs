using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Screen : MonoBehaviour
{
    [SerializeField] protected CanvasGroup CanvasGroup;
    [SerializeField] protected Button PlayButton;

    private void OnEnable()
    {
        PlayButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        PlayButton.onClick.RemoveListener(OnButtonClick);
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    public abstract void Open();

    public abstract void Close();
    
    protected abstract void OnButtonClick();
}

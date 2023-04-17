using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuScreen : MonoBehaviour
{
    [SerializeField] protected CanvasGroup CanvasGroup;
    [SerializeField] protected Button RestartButton;
    [SerializeField] protected Button ExitButton;
    [SerializeField] protected Button CreditsButton;
    [SerializeField] private SubStartScreen _creditsScreen;
    
    private void OnEnable()
    {
        _creditsScreen.TurnOffCanvasGroup();
        RestartButton.onClick.AddListener(OnRestartButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
        CreditsButton.onClick.AddListener(OnCreditsButtonClicked);
    }

    private void OnDisable()
    {
        RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
        ExitButton.onClick.RemoveListener(OnExitButtonClicked);
        CreditsButton.onClick.RemoveListener(OnCreditsButtonClicked);
    }

    protected abstract void OnRestartButtonClicked();

    public abstract void Open();
    
    public abstract void Close();

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    private void OnCreditsButtonClicked()
    {
        _creditsScreen.Open();
    }
}

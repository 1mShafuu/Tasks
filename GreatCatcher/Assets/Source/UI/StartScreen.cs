using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class StartScreen : MenuScreen
{
    public event UnityAction PlayButtonClicked;
    
    public override void Open()
    {
        CanvasGroup.alpha = 1;
        RestartButton.interactable = true;
    }

    public override void Close()
    {
        CanvasGroup.alpha = 0;
        RestartButton.interactable = false;
    }

    protected override void OnCreditsButtonClicked()
    {
        
    }

    protected override void OnRestartButtonClicked()
    {
        PlayButtonClicked?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartScreen : Screen
{
    public event UnityAction PlayButtonClicked;

    protected override void OnRestartButtonClicked()
    {
        PlayButtonClicked?.Invoke();
    }

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
}

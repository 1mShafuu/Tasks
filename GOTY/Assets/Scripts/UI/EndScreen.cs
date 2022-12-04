using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndScreen : Screen
{
    public event UnityAction RestartButtonClicked;
    
    protected override void OnButtonClicked()
    {
        RestartButtonClicked?.Invoke();
    }

    public override void Open()
    {
        CanvasGroup.alpha = 1;
        Button.interactable = true;
    }

    public override void Close()
    {
        CanvasGroup.alpha = 0;
        Button.interactable = false;
    }
}

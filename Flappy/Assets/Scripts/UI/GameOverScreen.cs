using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverScreen : Screen
{
    public event UnityAction RestartButtonClick;
    
    public override void Open()
    {
        CanvasGroup.alpha = 1;
        PlayButton.interactable = true;
    }

    public override void Close()
    {
        CanvasGroup.alpha = 0;
        PlayButton.interactable = false;
    }

    protected override void OnButtonClick()
    {
        RestartButtonClick?.Invoke();
    }
}

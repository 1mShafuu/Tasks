using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class StartScreen : MenuScreen
{
    public event Action PlayButtonClicked;

    public override void Open()
    {
        CanvasGroup.alpha = 1;
        CanvasGroup.interactable = true;
        CanvasGroup.blocksRaycasts = true;
        RestartButton.interactable = true;
    }

    public override void Close()
    {
        CanvasGroup.alpha = 0;
        CanvasGroup.interactable = false;
        CanvasGroup.blocksRaycasts = false;
        RestartButton.interactable = false;
    }

    protected override void OnRestartButtonClicked()
    {
        PlayButtonClicked?.Invoke();
    }
}

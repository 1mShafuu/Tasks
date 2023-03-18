using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpAlerts : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        Close();
    }

    public void Open()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}

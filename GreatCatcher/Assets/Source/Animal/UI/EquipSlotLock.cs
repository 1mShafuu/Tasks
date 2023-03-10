using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlotLock : MonoBehaviour
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
    }

    public void Close()
    {
        _canvasGroup.alpha = 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdNotification : PopUpAlerts
{
    [SerializeField] private Button _watchVideoButton;
    [SerializeField] private WatchAdArea _watchAdArea;

    public event Action WatchAdButtonClicked;
    
    private void OnEnable()
    {
        _watchAdArea.PlayerEntered += OnPlayerEntered;
        _watchAdArea.PlayerExit += OnPlayerExit;
        _watchVideoButton.interactable = false;
        _watchVideoButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _watchAdArea.PlayerEntered -= OnPlayerEntered;
        _watchAdArea.PlayerExit -= OnPlayerExit;
        _watchVideoButton.onClick.AddListener(OnButtonClicked);
    }

    private void OnPlayerEntered()
    {
        Open();
        _watchVideoButton.interactable = true;
    }

    private void OnPlayerExit()
    {
        Close();
        _watchVideoButton.interactable = false;
    }

    private void OnButtonClicked()
    {
        WatchAdButtonClicked?.Invoke();
    }
}

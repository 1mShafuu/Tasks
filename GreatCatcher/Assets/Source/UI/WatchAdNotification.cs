using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdNotification : SimpleMenu
{
    [SerializeField] private Button _watchVideoButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private WatchAdArea _watchAdArea;

    public event Action WatchAdButtonClicked;
    
    private void OnEnable()
    {
        _watchAdArea.PlayerEntered += OnPlayerEntered;
        _watchAdArea.PlayerExit += OnPlayerExit;
        _watchVideoButton.interactable = false;
        _watchVideoButton.onClick.AddListener(OnWatchVideoButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDisable()
    {
        _watchAdArea.PlayerEntered -= OnPlayerEntered;
        _watchAdArea.PlayerExit -= OnPlayerExit;
        _watchVideoButton.onClick.RemoveListener(OnWatchVideoButtonClicked);
        _exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    protected override void OnButtonClicked()
    {
        throw new NotImplementedException();
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
    
    private void OnWatchVideoButtonClicked()
    {
        WatchAdButtonClicked?.Invoke();
    }

    private void OnExitButtonClicked()
    {
        Close();
    }
}

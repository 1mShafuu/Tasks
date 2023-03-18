using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Chat[] _chats;
    [SerializeField] private Button _nextChatButton;
    [SerializeField] private Game _game;

    private int _currentChatNumber = 0;
    private int _lastChatNumber;

    public event Action TutorialEnded; 

    private void OnEnable()
    {
        _nextChatButton.onClick.AddListener(OnButtonClicked);
        _game.GameStarted += OnGameStarted;
        _lastChatNumber = _chats.Length - 1;
    }

    private void OnDisable()
    {
        _nextChatButton.onClick.RemoveListener(OnButtonClicked);
        _game.GameStarted -= OnGameStarted;
    }

    private void Start()
    {
        foreach (var chat in _chats)
        {
            chat.TryGetComponent(out PopUpAlerts alertToClose);
            alertToClose.Close();
        }
        
        _chats[_currentChatNumber].TryGetComponent(out PopUpAlerts alertToOpen);
        alertToOpen.Open();
        _nextChatButton.TryGetComponent(out PopUpAlerts buttonToOpen);
        buttonToOpen.Open();
    }

    private void OnButtonClicked()
    {
        if (_currentChatNumber == _lastChatNumber)
        {
            TutorialEnded?.Invoke();
            _chats[_currentChatNumber].TryGetComponent(out PopUpAlerts alertToClose);
            alertToClose.Close();
            _nextChatButton.TryGetComponent(out PopUpAlerts button);
            button.Close();
            Time.timeScale = 1;
        }
        else
        {
            _chats[_currentChatNumber].TryGetComponent(out PopUpAlerts alertToClose);
            alertToClose.Close();
            _currentChatNumber++;
            _chats[_currentChatNumber].TryGetComponent(out PopUpAlerts alertToOpen);
            alertToOpen.Open();
        }
    }

    private void OnGameStarted()
    {
        Time.timeScale = 0;
    }
}

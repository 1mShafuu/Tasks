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
        Time.timeScale = 0;
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
            chat.TryGetComponent(out SimpleMenu alertToClose);
            alertToClose.Close();
        }
        
        _chats[_currentChatNumber].TryGetComponent(out SimpleMenu alertToOpen);
        alertToOpen.Open();
        _nextChatButton.TryGetComponent(out SimpleMenu buttonToOpen);
        buttonToOpen.Open();
    }

    private void OnButtonClicked()
    {
        _chats[_currentChatNumber].TryGetComponent(out SimpleMenu alertToClose);
        _nextChatButton.TryGetComponent(out SimpleMenu button);
        
        if (_currentChatNumber == _lastChatNumber)
        {
            TutorialEnded?.Invoke();
            alertToClose.Close();
            button.Close();
            Time.timeScale = 1;
            _currentChatNumber = 0;
        }
        else
        {
            alertToClose.Close();
            _currentChatNumber++;
            _chats[_currentChatNumber].TryGetComponent(out SimpleMenu alertToOpen);
            alertToOpen.Open();
        }
    }

    private void OnGameStarted()
    {
       // Time.timeScale = 0;
    }
}

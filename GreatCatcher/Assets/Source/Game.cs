using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndScreen _winScreen;
    
    private Wallet _wallet;

    public event Action GameStarted; 
    public event Action GameEnded;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        _wallet = _player.GetComponent<Wallet>();
        _startScreen.PlayButtonClicked += OnPlayButtonClicked;
        _wallet.BalanceChanged += OnBalanceChanged;
        Time.timeScale = 0;
        _winScreen.TurnOffCanvasGroup();
        _startScreen.Open();
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClicked -= OnPlayButtonClicked;
    }
    
    private void OnPlayButtonClicked()
    {
        _startScreen.Close();
        StartGame();
    }
    
    private void StartGame()
    {
        GameStarted?.Invoke();
    }

    private void OnBalanceChanged(int value)
    {
        const int winCondition = 1000000;

        if (value >= winCondition)
        {
            GameEnded?.Invoke();
            _winScreen.Open();
        }
    }
}
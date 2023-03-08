using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private StartScreen _startScreen;

    private void Awake()
    {
        YandexGamesSdk.CallbackLogging = true;
    }

    private void OnEnable()
    {
        _startScreen.PlayButtonClicked += OnPlayButtonClicked;
        Time.timeScale = 0;
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
        Time.timeScale = 1;
    }
}
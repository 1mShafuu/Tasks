using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndScreen _endScreen;

    private void Start()
    {
        Time.timeScale = 0;
        _startScreen.Open();
    }

    private void OnEnable()
    {
        _startScreen.PlayButtonClicked += OnPlayButtonClicked;
        _endScreen.RestartButtonClicked += OnRestartButtonClicked;
        _player.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        _startScreen.PlayButtonClicked -= OnPlayButtonClicked;
        _endScreen.RestartButtonClicked -= OnRestartButtonClicked;
        _player.GameOver -= OnGameOver;
    }

    private void OnPlayButtonClicked()
    {
        _startScreen.Close();
        StartGame();
    }
    
    private void OnRestartButtonClicked()
    {
        _endScreen.Close();
        _levelGenerator.ResetPool();
        _levelGenerator.Restart();
        StartGame();
    }

    private void StartGame()
    {
        Time.timeScale = 1;
        _player.Reset();
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        _endScreen.Open();
    }
}

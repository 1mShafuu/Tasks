using System;
using UnityEngine;
using Agava.YandexGames;

public class Game : MonoBehaviour
{
    private const int WinCondition = 1000000;
    public const int MaxPoints = 105000000;
    
    [SerializeField] private Player _player;
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private SubStartScreen _winScreen;
    [SerializeField] private ScoreCounter _scoreCounter;
    
    private Wallet _wallet;
    
    public int ScoreToLeaderboard { get; private set; }
    
    public event Action GameStarted; 
    public event Action GameEnded;

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
        Time.timeScale = 1;
#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerAccount.RequestPersonalProfileDataPermission();
#endif
        // PlayerAccount.RequestPersonalProfileDataPermission();
    }

    private void OnBalanceChanged(int value)
    {
        if (value >= WinCondition)
        {
            _scoreCounter.ResetScore();
            ScoreToLeaderboard = _scoreCounter.Score;
            GameEnded?.Invoke();
            _winScreen.Open();
        }
    }
}
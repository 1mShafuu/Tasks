using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using UnityEngine.UI;

public class ProgressSaver : MonoBehaviour
{
    [SerializeField] private Button _saveButton;
    [SerializeField] private Player _player;
    [SerializeField] private GameLeaderBoard _gameLeaderBoard;
    [SerializeField] private AnimalsUnloader _animalsUnloader;
    [SerializeField] private Game _game;
    [SerializeField] private PlayerInfoHolder _playerInfoHolder;
    
    private Wallet _wallet;
    private int _currentLevelYard = 1;
    private PlayerInfo _playerInfoSavingBlueprint;

    private void Awake()
    {
        StartCoroutine(GetPlayerData());
    }

    private void OnEnable()
    {
        _wallet = _player.GetComponent<Wallet>();
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _animalsUnloader.YardChose += OnYardChosen;
        _game.GameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _animalsUnloader.YardChose -= OnYardChosen;
        _game.GameEnded -= OnGameEnded;
    }
    
    private IEnumerator GetPlayerData()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
      yield return YandexGamesSdk.Initialize();

       if (PlayerAccount.IsAuthorized && YandexGamesSdk.IsInitialized)
       {
          PlayerAccount.GetPlayerData(HandleJson);
       }
#else
        yield return null;
#endif
    }
    
    private void OnSaveButtonClicked()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
        {
            _playerInfoSavingBlueprint = new PlayerInfo();
            _playerInfoSavingBlueprint.Wallet = _wallet.Money;
            _playerInfoSavingBlueprint.Level = _player.Level;
            _playerInfoSavingBlueprint.Yard = _currentLevelYard;
            PlayerInfoJson playerInfoJson = new PlayerInfoJson();
            PlayerAccount.SetPlayerData(playerInfoJson.SaveJSONToString(_playerInfoSavingBlueprint));
            Debug.Log($"{_currentLevelYard}  {_playerInfoSavingBlueprint.Wallet}  {_playerInfoSavingBlueprint.Level}");
        }
#endif
    }

    private void OnYardChosen(int yardLevel)
    {
        if (yardLevel is >= 1 and <= 3)
        {
            _currentLevelYard = yardLevel;
        }
    }

    private void OnGameEnded()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
      if (PlayerAccount.IsAuthorized)
        {
            _playerInfoSavingBlueprint = new PlayerInfo
            {
                Wallet = 0,
                Level = 1,
                Yard = 1
            };
            PlayerInfoJson playerInfoJson = new PlayerInfoJson();
            PlayerAccount.SetPlayerData(playerInfoJson.SaveJSONToString(_playerInfoSavingBlueprint));
            Debug.Log($"{_currentLevelYard}  {_playerInfoSavingBlueprint.Wallet}  {_playerInfoSavingBlueprint.Level}");
        }  
#endif
    }
    
    private void HandleJson(string possibleData)
    {
        var playerStats = PlayerInfoJson.CreateFromJSONFile(possibleData);
       // Debug.Log($" HandleJSON  {playerStats.Yard}    {playerStats.Wallet}  {playerStats.Level}");
        _playerInfoHolder.GetPlayerStats(playerStats);
    }
}
